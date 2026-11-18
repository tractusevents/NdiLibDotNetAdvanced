"""Print the symbol and enum contract from a local NDI Advanced SDK installation.

Run from a Visual Studio developer prompt on Windows, or with c++ on PATH on Unix.
"""
from pathlib import Path
import argparse
import os
import re
import subprocess
import tempfile

parser = argparse.ArgumentParser()
parser.add_argument("include", type=Path)
parser.add_argument("--output", type=Path)
args = parser.parse_args()
include = args.include.resolve()
functions = set()
members = {}
for header in sorted(include.glob("*.h")):
    source = header.read_text(encoding="utf-8-sig")
    source = re.sub(r"//[^\n]*|/\*.*?\*/", "", source, flags=re.DOTALL)
    for declaration in re.findall(
        r"^[ \t]*(?:PROCESSINGNDILIB_API|PROCESSINGNDILIB_ADVANCED_API|PROCESSINGNDILIBFILE_API)\s+([^;#{}]+);",
        source, flags=re.MULTILINE
    ):
        name = re.search(r"\b(NDIlib_\w+)\s*\(", declaration)
        if name:
            functions.add(name[1])
    for match in re.finditer(r"\benum\s*(NDIlib_\w+)?\s*\{([^}]+)\}", source):
        # At the start of an enumerator, skip commas inside FourCC macro invocations.
        names = re.findall(r"^\s*(NDIlib_\w+)\s*(?==|,|$)", match[2], flags=re.MULTILINE)
        for name in names:
            members[name] = match[1] or "file_buffer_size"

with tempfile.TemporaryDirectory() as directory:
    work = Path(directory)
    probe = work / "values.cpp"
    program = work / ("values.exe" if os.name == "nt" else "values")
    lines = ['#include <cstdio>', '#include "Processing.NDI.Advanced.h"',
             '#include "Processing.NDI.Lib.File.h"', 'int main() {']
    for name, enum in sorted(members.items()):
        lines.append(f'    std::printf("{enum}\\t{name}\\t%u\\n", static_cast<unsigned>({name}));')
    probe.write_text("\n".join(lines + ["}"]), encoding="utf-8")
    if os.name == "nt":
        command = ["cl", "/nologo", "/EHsc", f"/I{include}", str(probe), f"/Fe:{program}"]
    else:
        command = ["c++", "-std=c++11", "-I", str(include), str(probe), "-o", str(program)]
    subprocess.run(command, cwd=work, stdout=subprocess.DEVNULL, check=True)
    values = subprocess.check_output([program], text=True).splitlines()
contract = "\n".join(sorted(functions) + sorted(values)) + "\n"
if args.output:
    args.output.write_text(contract, encoding="utf-8", newline="\n")
else:
    print(contract, end="")
