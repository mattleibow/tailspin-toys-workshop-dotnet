#!/bin/bash

# Setup environment: .NET restore

# Determine project root
if [[ $(basename $(pwd)) == "scripts" ]]; then
    PROJECT_ROOT=$(pwd)/..
else
    PROJECT_ROOT=$(pwd)
fi

cd "$PROJECT_ROOT" || exit 1

echo "Restoring .NET dependencies..."
dotnet restore

echo "Environment setup complete."
