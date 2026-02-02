#!/usr/bin/env bash
# Build script for Linux/macOS

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SOLUTION_FILE="$SCRIPT_DIR/Strat.slnx"
OUTPUT_DIR="$SCRIPT_DIR/artifacts"
CONFIGURATION="Release"

# Parse arguments
CLEAN=false
RESTORE=false
BUILD=false
PUBLISH=false

while [[ $# -gt 0 ]]; do
    case $1 in
        --clean|-c)
            CLEAN=true
            shift
            ;;
        --restore|-r)
            RESTORE=true
            shift
            ;;
        --build|-b)
            BUILD=true
            shift
            ;;
        --publish|-p)
            PUBLISH=true
            shift
            ;;
        --configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        --help|-h)
            echo "Usage: $0 [options]"
            echo "Options:"
            echo "  --clean, -c        Clean build artifacts"
            echo "  --restore, -r      Restore NuGet packages"
            echo "  --build, -b        Build the solution"
            echo "  --publish, -p      Publish the application"
            echo "  --configuration    Build configuration (Debug|Release)"
            echo "  --help, -h         Show this help message"
            exit 0
            ;;
        *)
            echo "Unknown option: $1"
            exit 1
            ;;
    esac
done

# Default: if no switches specified, do Restore + Build
if [ "$CLEAN" = false ] && [ "$RESTORE" = false ] && [ "$BUILD" = false ] && [ "$PUBLISH" = false ]; then
    RESTORE=true
    BUILD=true
fi

print_header() {
    echo ""
    echo "========================================"
    echo " $1"
    echo "========================================"
}

if [ "$CLEAN" = true ]; then
    print_header "Cleaning..."
    rm -rf "$OUTPUT_DIR"
    dotnet clean "$SOLUTION_FILE" -c "$CONFIGURATION" --nologo -v q
    echo "Clean completed."
fi

if [ "$RESTORE" = true ]; then
    print_header "Restoring packages..."
    dotnet restore "$SOLUTION_FILE" --nologo
    echo "Restore completed."
fi

if [ "$BUILD" = true ]; then
    print_header "Building ($CONFIGURATION)..."
    dotnet build "$SOLUTION_FILE" -c "$CONFIGURATION" --no-restore --nologo
    echo "Build completed."
fi

if [ "$PUBLISH" = true ]; then
    print_header "Publishing ($CONFIGURATION)..."
    PUBLISH_DIR="$OUTPUT_DIR/publish"
    dotnet publish "$SCRIPT_DIR/src/Host/Strat.Host/Strat.Host.csproj" \
        -c "$CONFIGURATION" \
        -o "$PUBLISH_DIR" \
        --no-restore \
        --nologo
    echo "Publish completed. Output: $PUBLISH_DIR"
fi

echo ""
echo "All tasks completed successfully!"
