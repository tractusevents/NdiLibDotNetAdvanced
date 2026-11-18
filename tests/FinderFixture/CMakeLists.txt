cmake_minimum_required(VERSION 3.15)
project(FinderFixture LANGUAGES CXX)
add_library(finder-fixture SHARED finder.cpp)
target_compile_features(finder-fixture PRIVATE cxx_std_11)
set_target_properties(finder-fixture PROPERTIES PREFIX "")
enable_testing()
add_test(NAME finder COMMAND dotnet run --project "${CMAKE_CURRENT_SOURCE_DIR}/../AbiLayout/AbiLayout.csproj"
    --configuration Release -- --finder $<TARGET_FILE:finder-fixture>)
