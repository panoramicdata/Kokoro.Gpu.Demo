#!/bin/bash
# Example shell script demonstrating various Kokoro TTS commands

echo "== Kokoro TTS Demo Examples =="
echo

echo "1. Listing available voices:"
dotnet run -- --list-voices
echo

echo "2. Basic text synthesis with default voice:"
dotnet run -- --text "Hello, this is a basic example of Kokoro TTS."
echo

echo "3. Using a specific voice:"
dotnet run -- --text "This example uses a different voice." --voice af_heart
echo

echo "4. Multilingual example (French):"
dotnet run -- --text "Bonjour! Ceci est un exemple en français." --voice fr_male
echo

echo "5. Professional announcement:"
dotnet run -- --text "Ladies and gentlemen, welcome to today's presentation." --voice bm_george
echo

echo "6. Starting interactive mode:"
echo "(Press Ctrl+C to skip interactive mode)"
sleep 3
dotnet run -- --interactive

echo
echo "Demo completed!"