#include <atomic>
#include <chrono>
#include <cstring>
#include <thread>

#ifdef _WIN32
#define API extern "C" __declspec(dllexport)
#else
#define API extern "C" __attribute__((visibility("default")))
#endif

struct Source
{
    const char* name;
    const char* address;
};

struct Settings
{
    bool local;
    const char* groups;
    const char* extraIps;
};

static const Source initial[] = {{"HOST (One)", "url1"}, {"HOST (One)", "url2"}};
static const Source reordered[] = {initial[1], initial[0]};
static const Source changed[] = {initial[1], {"HOST (One)", "url3"}};
static std::atomic<int> requested{0};
static std::atomic<int> completed{0};
static std::atomic<int> mode{0};
static std::atomic<int> delivered{0};

API void* NDIlib_find_create_v2(const Settings* settings)
{
    if ((settings->groups && std::strcmp(settings->groups, "g,g") != 0)
        || (settings->extraIps && std::strcmp(settings->extraIps, "1,2") != 0))
    {
        return nullptr;
    }
    return &mode;
}

API void NDIlib_find_destroy(void*) {}

API int finder_change(int next)
{
    mode.store(next);
    return ++requested;
}

API int finder_completed()
{
    return completed.load();
}

API bool NDIlib_find_wait_for_sources(void*, unsigned)
{
    completed.store(delivered.load());
    std::this_thread::sleep_for(std::chrono::milliseconds(2));
    const auto revision = requested.load();
    if (revision == delivered.load())
    {
        return false;
    }
    delivered.store(revision);
    return true;
}

API const Source* NDIlib_find_get_current_sources(void*, unsigned* count)
{
    const auto current = mode.load();
    *count = current == 3 ? 0 : 2;
    switch (current)
    {
        case 1: return reordered;
        case 2: return changed;
        case 3: return nullptr;
        default: return initial;
    }
}
