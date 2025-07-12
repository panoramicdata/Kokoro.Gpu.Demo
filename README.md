# 🎙️ Kokoro GPU TTS Demo

A powerful, self-contained demonstration of the **Kokoro Text-to-Speech (TTS)** engine, featuring high-quality neural speech synthesis with GPU acceleration. This demo showcases natural-sounding voice generation with multiple language support and an intuitive interface.

## ✨ Features

- 🚀 **GPU-Accelerated**: High-performance neural TTS with GPU optimization
- 🌍 **Multi-Language Support**: Multiple voices across different languages
- 🎯 **Dual Mode Operation**: Interactive mode and command-line interface
- 🔍 **Smart Voice Discovery**: Search and filter voices with autocomplete
- ⚡ **Real-time Synthesis**: Instant audio playback for immediate feedback
- 🛠️ **Developer Friendly**: Clean API with comprehensive error handling
- 📁 **WAV Export**: Planned for future release - save generated speech to files

## 🚀 Quick Start

### Prerequisites

- .NET 9.0 or later
- GPU with CUDA support (recommended for optimal performance)
- Audio output device for playback

### Installation

1. Clone or download this repository
2. Build the project:dotnet build3. Run the demo:dotnet run## 📖 Usage Guide

### Interactive Mode (Default)

When you run the application without arguments, it starts in interactive mode:

``` bash
dotnet run

Loading Kokoro TTS model...
Model loaded successfully!
Welcome to Kokoro TTS Interactive Mode!
Using voice: bm_lewis (English)
Type 'help' for available commands or just start typing to speak text.

🔊 > Hello, this is a demonstration of Kokoro TTS!
🔊 > voice af_heart
✅ Switched to voice: af_heart (English)

🔊 > wav on
💡 WAV output mode enabled (visual indicator only).
    Note: File saving functionality will be added in a future version.
📁 > This text will be spoken, not saved yet.
⚠️  WAV output mode is enabled but file saving is not yet implemented.
    Playing audio directly instead...
📁 > wav off
✅ WAV output disabled. Audio will be played directly.
🔊 > exit
Goodbye!
```
#### Interactive Mode Commands

| Command | Description | Example |
|---------|-------------|---------|
| `<text>` | Speak the entered text | `Hello world!` |
| `voice <name>` | Change voice (with Tab autocomplete) | `voice bm_lewis` |
| `voices` | List all available voices | `voices` |
| `voices <prefix>` | Filter voices by prefix | `voices af_` |
| `search <term>` | Search voices by name/language | `search english` |
| `wav on` | Enable WAV mode (visual indicator) | `wav on` |
| `wav off` | Disable WAV mode | `wav off` |
| `wav status` | Show current WAV mode status | `wav status` |
| `help` | Display help information | `help` |
| `exit` / `quit` | Exit the application | `exit` |

### Command-Line Mode

For automation, scripting, or quick one-off synthesis:

#### Basic Text Synthesis
# Synthesize and play immediately
``` bash
dotnet run -- --text "Hello, welcome to Kokoro TTS!"
```

# Use a specific voice
``` bash
dotnet run -- --text "Bonjour le monde!" --voice fr_male
```

#### WAV File Output (Planned)# Future functionality - will save to WAV file
``` bash
dotnet run -- --text "This is a test" --voice bm_lewis --output "greeting.wav"
```

# Currently shows: "WAV file output is not yet implemented, playing directly instead"#### Voice Management# List all available voices
``` bash
dotnet run -- --list-voices
```

# Force interactive mode
``` bash
dotnet run -- --interactive
```

``` bash
dotnet run -- --list-voices

Available voices:

  af_heart             (English)
  af_sky               (English)
  am_adam              (English)
  am_michael           (English)
  bf_emma              (English)
  bf_isabella          (English)
  bm_george            (English)
  bm_lewis             (English)
  fr_male              (French)
  de_female            (German)
  
Total: 10 voices
```

``` bash
dotnet run -- --text "Hello from the command line!" --voice bm_lewis

Loading Kokoro TTS model...
Model loaded successfully!
Using voice: bm_lewis (English)
Synthesizing: "Hello from the command line!"
✅ Audio playback completed.
```

## 🎛️ Command-Line Reference

| Option | Short | Description | Example |
|--------|-------|-------------|---------|
| `--text` | `-t` | Text to synthesize | `--text "Hello world"` |
| `--voice` | `-v` | Voice to use | `--voice bm_lewis` |
| `--output` | `-o` | Output WAV file path (planned) | `--output speech.wav` |
| `--list-voices` | `-l` | List available voices | `--list-voices` |
| `--interactive` | `-i` | Start interactive mode | `--interactive` |

## 🎯 Example Use Cases

### 1. Voice Testing and Experimentation# Test different voices quickly
``` bash
dotnet run -- --text "Hello, I am testing different voices" --voice bm_george
dotnet run -- --text "The same text with a different voice" --voice af_sky
```

# Compare voices in interactive mode
``` bash
dotnet run
```

# Then use: voice bm_lewis, voice af_heart, etc.
### 2. Content Creation (Future with WAV Support)# Generate narration for a video (planned functionality)
``` bash
dotnet run -- --text "Welcome to our tutorial series" --voice bm_george --output "intro.wav"
```

# Create multiple audio files with different voices (planned)
``` bash
dotnet run -- --text "English version" --voice bm_lewis --output "en.wav"
dotnet run -- --text "Version française" --voice fr_male --output "fr.wav"### 3. Interactive Voice Development
```

Start interactive mode and experiment with different voices:
``` bash
dotnet run
```
# Then use the interactive commands to test various voices and settings

### 4. Batch Processing (Current Implementation)# Process multiple texts with the same voice (direct playback)
``` bash
dotnet run -- --text "First sentence" --voice bm_lewis
dotnet run -- --text "Second sentence" --voice bm_lewis
dotnet run -- --text "Third sentence" --voice bm_lewis## 🔧 Configuration
```

### Current Features

- **Real-time Audio**: Direct playback through system audio
- **Voice Selection**: Support for all available Kokoro voices
- **Interactive Shell**: Tab completion and voice search

### Planned Features

- 📁 **WAV File Output**: Save generated speech to high-quality WAV files
- 🎛️ **Audio Configuration**: Customizable sample rates and audio settings
- 📦 **Batch Processing**: Process multiple texts in a single command
- 🎨 **Voice Previews**: Short samples to preview voice characteristics
- 📊 **Usage Statistics**: Track synthesis performance and usage
- 🌐 **API Mode**: HTTP server for remote TTS requests

### Voice Selection

The application automatically selects `bm_lewis` as the default voice. If unavailable, it falls back to the first available voice.

## 🎨 Voice Showcase

Here are examples of different voices you can use:

| Voice Name | Language | Gender | Characteristics |
|------------|----------|---------|----------------|
| `bm_lewis` | English | Male | Clear, professional tone |
| `af_heart` | English | Female | Warm, friendly voice |
| `bm_george` | English | Male | Deep, authoritative |
| `bf_emma` | English | Female | Young, energetic |
| `fr_male` | French | Male | Native French pronunciation |
| `de_female` | German | Female | Native German pronunciation |

**Try these examples:**

# Professional announcement
``` bash
dotnet run -- --text "Ladies and gentlemen, welcome to the presentation" --voice bm_george
```

# Friendly greeting
``` bash
dotnet run -- --text "Hi there! Thanks for using Kokoro TTS!" --voice af_heart
```

# Multilingual content
``` bash
dotnet run -- --text "Bonjour, comment allez-vous?" --voice fr_male## 🛠️ Development
```

### Building from Source

``` bash
git clone https://github.com/panoramic-data/Kokoru.Gpu.Demo.git
cd Kokoro.GPU.Demo
dotnet restore
dotnet build --configuration Release
```

### Dependencies
- **KokoroSharp.GPU**: Core TTS engine
- **System.CommandLine**: Command-line interface framework
- **Nerdbank.GitVersioning**: Automatic versioning

### Version Information
This demo uses Nerdbank.GitVersioning for automatic semantic versioning based on Git history.

## 📝 Tips & Tricks

1. **Tab Completion**: In interactive mode, type `voice ` and press Tab to cycle through available voices
2. **Voice Discovery**: Use `search` command to find voices by language or characteristics
3. **Performance**: First synthesis may take longer due to model loading; subsequent calls are much faster
4. **Quick Testing**: Use command-line mode for testing single phrases quickly
5. **Interactive Exploration**: Use interactive mode to experiment with different voices and settings

## 🎭 Virtual Interactive Session

Here's what a typical interactive session looks like:

``` bash
Loading Kokoro TTS model...
Model loaded successfully!
Welcome to Kokoro TTS Interactive Mode!
Using voice: bm_lewis (English)
Type 'help' for available commands or just start typing to speak text.

🔊 > Hello, I'm testing the Kokoro TTS system.
🔊 > voices af_
Voices starting with 'af_' (2 found):
  • af_heart (English)
  • af_sky (English)

🔊 > voice af_sky
✅ Switched to voice: af_sky (English)

🔊 > This voice sounds different from the previous one.
🔊 > wav on
💡 WAV output mode enabled (visual indicator only).
    Note: File saving functionality will be added in a future version.
📁 > Let me test the WAV mode indicator.
⚠️  WAV output mode is enabled but file saving is not yet implemented.
    Playing audio directly instead...
📁 > wav status
WAV output mode: ON (indicator only)
Note: Actual file saving is not yet implemented.
📁 > search french
🔍 Found 1 voice(s) matching 'french':
  • fr_male (French)

📁 > voice fr_male
✅ Switched to voice: fr_male (French)

📁 > Bonjour, comment ça va aujourd'hui?
⚠️  WAV output mode is enabled but file saving is not yet implemented.
    Playing audio directly instead...
📁 > help
Interactive Mode Commands:
  • Type text to speak it
  • 'voice <name>' - Change voice (exact name required, Tab for completion)
  • 'voices' - List all available voices
  • 'voices <prefix>' - List voices starting with prefix (e.g., 'voices bf_')
  • 'search <term>' - Search for voices by name or language
  • 'wav on' - Enable WAV file output (currently shows notification only)
  • 'wav off' - Disable WAV file output
  • 'wav status' - Show current WAV output status
  • 'help' - Show this help
  • 'exit' or 'quit' - Exit the program

💡 Tip: Use Tab after 'voice ' to auto-complete voice names!
💡 Note: WAV output is planned for a future version

📁 > exit
Goodbye!
```

## 🚧 Roadmap

### Current Version (v1.0)
- ✅ Interactive TTS with voice switching
- ✅ Command-line interface
- ✅ Voice search and filtering
- ✅ Tab completion for voice names
- ✅ Real-time audio playback

### Planned Features
- 📁 **WAV File Output**: Save generated speech to high-quality WAV files
- 🎛️ **Audio Configuration**: Customizable sample rates and audio settings
- 📦 **Batch Processing**: Process multiple texts in a single command
- 🎨 **Voice Previews**: Short samples to preview voice characteristics
- 📊 **Usage Statistics**: Track synthesis performance and usage
- 🌐 **API Mode**: HTTP server for remote TTS requests

## 📄 License and Copyright

MIT License (MIT)
Copyright © 2025 Panoramic Data Limited. All rights reserved.

---

**Happy Voice Synthesis!** 🎉

For more information about the Kokoro TTS engine, visit the [KokoroSharp documentation](https://github.com/dranger003/KokoroSharp).

### Contributing

This is a demonstration project. For issues or feature requests related to the underlying TTS engine, please visit the [KokoroSharp.GPU repository](https://github.com/dranger003/KokoroSharp).
