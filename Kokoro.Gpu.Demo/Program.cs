using KokoroSharp;

namespace Kokoro.Gpu.Demo;

public class Program
{
	public static async Task Main()
	{
		try
		{
			await RunTtsDemoAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}

	private static async Task RunTtsDemoAsync()
	{
		Console.WriteLine("Welcome to Kokoro TTS Demo!");
		Console.WriteLine("Loading model...");

		var tts = await LoadModelWithRetryAsync();
		Console.WriteLine("Model loaded successfully!");

		var currentVoice = GetDefaultVoice();
		if (currentVoice == null)
		{
			Console.WriteLine("No voices available. Exiting...");
			return;
		}

		Console.WriteLine($"Using voice: {currentVoice.Name} ({currentVoice.Language})");
		Console.WriteLine("Type 'help' for available commands or just start typing to speak text.");

		await ProcessUserInputAsync(tts, currentVoice);
	}

	private static async Task<KokoroTTS> LoadModelWithRetryAsync()
	{
		const int maxRetries = 3;
		for (int attempt = 1; attempt <= maxRetries; attempt++)
		{
			try
			{
				return await KokoroTTS.LoadModelAsync();
			}
			catch (Exception ex) when (attempt < maxRetries)
			{
				Console.WriteLine($"Failed to load model (attempt {attempt}/{maxRetries}): {ex.Message}");
				Console.WriteLine("Retrying...");
				await Task.Delay(1000); // Wait 1 second before retry
			}
		}

		// Final attempt without catching exception
		return await KokoroTTS.LoadModelAsync();
	}

	private static void DisplayAvailableVoices(string? prefixFilter = null)
	{
		var voices = KokoroVoiceManager.Voices.ToList();

		if (voices.Count == 0)
		{
			Console.WriteLine("No voices available.");
			return;
		}

		// Apply prefix filter if provided
		if (!string.IsNullOrEmpty(prefixFilter))
		{
			var filterLower = prefixFilter.ToLowerInvariant();
			voices = [.. voices.Where(v => v.Name.ToLowerInvariant().StartsWith(filterLower, StringComparison.Ordinal))];

			if (voices.Count == 0)
			{
				Console.WriteLine($"No voices found starting with '{prefixFilter}'.");
				Console.WriteLine("💡 Try a different prefix or type 'voices' to see all available voices.");
				return;
			}

			Console.WriteLine($"\nVoices starting with '{prefixFilter}' ({voices.Count} found):");
		}
		else
		{
			Console.WriteLine("\nAvailable voices:");
		}

		foreach (var voice in voices.OrderBy(v => v.Language).ThenBy(v => v.Name))
		{
			Console.WriteLine($"  • {voice.Name} ({voice.Language})");
		}

		Console.WriteLine();
	}

	private static dynamic? GetDefaultVoice()
	{
		// Try to get the preferred voice first
		var preferredVoice = KokoroVoiceManager.GetVoice("bm_lewis");
		if (preferredVoice != null)
		{
			return preferredVoice;
		}

		// If preferred voice not available, get the first available voice
		var firstVoice = KokoroVoiceManager.Voices.FirstOrDefault();
		if (firstVoice != null)
		{
			Console.WriteLine($"⚠️  Default voice 'bm_lewis' not found. Using '{firstVoice.Name}' instead.");
			return firstVoice;
		}

		// No voices available at all
		return null;
	}

	private static void ShowHelp()
	{
		Console.WriteLine("\nCommands:");
		Console.WriteLine("  • Type text to speak it");
		Console.WriteLine("  • 'voice <name>' - Change voice (exact name required, Tab for completion)");
		Console.WriteLine("  • 'voices' - List all available voices");
		Console.WriteLine("  • 'voices <prefix>' - List voices starting with prefix (e.g., 'voices bf_')");
		Console.WriteLine("  • 'search <term>' - Search for voices by name or language");
		Console.WriteLine("  • 'help' - Show this help");
		Console.WriteLine("  • 'exit' or 'quit' - Exit the program");

		Console.WriteLine();
		Console.WriteLine("💡 Tip: Use Tab after 'voice ' to auto-complete voice names!");

		Console.WriteLine();
	}

	private static async Task ProcessUserInputAsync(KokoroTTS tts, dynamic initialVoice)
	{
		var currentVoice = initialVoice;

		while (true)
		{
			Console.Write("> ");
			var input = ReadLineWithTabCompletion()?.Trim();

			if (string.IsNullOrEmpty(input))
				continue;

			// Handle exit commands
			if (IsExitCommand(input))
			{
				Console.WriteLine("Goodbye!");
				break;
			}

			// Handle voice change command
			if (input.StartsWith("voice ", StringComparison.OrdinalIgnoreCase))
			{
				var result = HandleVoiceChange(input, currentVoice);
				currentVoice = result.Item1;

				// If voice change was successful, provide feedback
				if (result.Item2)
				{
					Console.WriteLine();
				}

				continue;
			}

			// Handle voices command (with optional prefix filter)
			if (input.StartsWith("voices", StringComparison.OrdinalIgnoreCase))
			{
				HandleVoicesCommand(input);
				continue;
			}

			// Handle other commands
			switch (input.ToLowerInvariant())
			{
				case "help":
					ShowHelp();
					continue;
			}

			// Handle search command for voices
			if (input.StartsWith("search ", StringComparison.OrdinalIgnoreCase))
			{
				HandleVoiceSearch(input);
				continue;
			}

			// Speak the text
			await SpeakTextAsync(tts, input, currentVoice);
		}
	}

	private static bool IsExitCommand(string input) =>
		input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
		input.Equals("quit", StringComparison.OrdinalIgnoreCase);

	private static (dynamic voice, bool success) HandleVoiceChange(string input, dynamic currentVoice)
	{
		var voiceName = input[6..].Trim();

		if (string.IsNullOrEmpty(voiceName))
		{
			Console.WriteLine("❌ Error: Please specify a voice name.");
			Console.WriteLine("💡 Example: voice af_heart");
			Console.WriteLine("💡 Tip: Type 'voices' to see all available options.");
			return (currentVoice, false);
		}

		// Try exact match first
		var newVoice = KokoroVoiceManager.GetVoice(voiceName);
		if (newVoice != null)
		{
			Console.WriteLine($"✅ Switched to voice: {newVoice.Name} ({newVoice.Language})");
			return (newVoice, true);
		}

		// If exact match fails, try to find similar voices
		var availableVoices = KokoroVoiceManager.Voices.ToList();
		var similarVoices = FindSimilarVoices(voiceName, availableVoices);

		Console.WriteLine($"❌ Voice '{voiceName}' not found.");

		if (similarVoices.Count != 0)
		{
			Console.WriteLine("🔍 Did you mean one of these?");
			foreach (var voice in similarVoices.Take(3)) // Show top 3 matches
			{
				Console.WriteLine($"  • {voice.Name} ({voice.Language})");
			}
		}
		else
		{
			Console.WriteLine("📋 Available voices:");
			foreach (var voice in availableVoices.Take(5)) // Show first 5 voices
			{
				Console.WriteLine($"  • {voice.Name} ({voice.Language})");
			}

			if (availableVoices.Count > 5)
			{
				Console.WriteLine($"  ... and {availableVoices.Count - 5} more (type 'voices' to see all)");
			}
		}

		Console.WriteLine("💡 Tip: Voice names are case-sensitive. Try copying the exact name from the list above.");
		Console.WriteLine("💡 You can also use 'voices <prefix>' to filter voices (e.g., 'voices af_').");

		return (currentVoice, false);
	}

	private static List<dynamic> FindSimilarVoices(string searchTerm, IEnumerable<dynamic> availableVoices)
	{
		var searchLower = searchTerm.ToLowerInvariant();

		return [.. availableVoices
			.Where(voice =>
				voice.Name.ToLowerInvariant().Contains(searchLower, StringComparison.Ordinal) ||
				voice.Language.ToLowerInvariant().Contains(searchLower, StringComparison.Ordinal) ||
				LevenshteinDistance(voice.Name.ToLowerInvariant(), searchLower) <= 2)
			.OrderBy(voice => LevenshteinDistance(voice.Name.ToLowerInvariant(), searchLower))];
	}

	private static int LevenshteinDistance(string source, string target)
	{
		if (string.IsNullOrEmpty(source))
			return string.IsNullOrEmpty(target) ? 0 : target.Length;

		if (string.IsNullOrEmpty(target))
			return source.Length;

		var matrix = new int[source.Length + 1, target.Length + 1];

		// Initialize first row and column
		for (int i = 0; i <= source.Length; i++)
			matrix[i, 0] = i;
		for (int j = 0; j <= target.Length; j++)
			matrix[0, j] = j;

		// Fill the matrix
		for (int i = 1; i <= source.Length; i++)
		{
			for (int j = 1; j <= target.Length; j++)
			{
				int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
				matrix[i, j] = Math.Min(
					Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
					matrix[i - 1, j - 1] + cost);
			}
		}

		return matrix[source.Length, target.Length];
	}

	private static async Task SpeakTextAsync(KokoroTTS tts, string text, dynamic voice)
	{
		try
		{
			// Add a small delay to make the experience feel more responsive
			await Task.Run(() => tts.SpeakFast(text, voice));
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error speaking text: {ex.Message}");
		}
	}

	private static void HandleVoiceSearch(string input)
	{
		var searchTerm = input[7..].Trim();

		if (string.IsNullOrEmpty(searchTerm))
		{
			Console.WriteLine("❌ Please specify a search term.");
			Console.WriteLine("💡 Example: search english");
			return;
		}

		var availableVoices = KokoroVoiceManager.Voices.ToList();
		var matchingVoices = FindSimilarVoices(searchTerm, availableVoices);

		if (matchingVoices.Count == 0)
		{
			Console.WriteLine($"🔍 No voices found matching '{searchTerm}'.");
			Console.WriteLine("💡 Try a different search term or type 'voices' to see all available voices.");
			return;
		}

		Console.WriteLine($"🔍 Found {matchingVoices.Count} voice(s) matching '{searchTerm}':");
		foreach (var voice in matchingVoices)
		{
			Console.WriteLine($"  • {voice.Name} ({voice.Language})");
		}

		Console.WriteLine();
		Console.WriteLine("💡 To use a voice, type: voice <exact_name>");
	}

	private static void HandleVoicesCommand(string input)
	{
		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length == 1)
		{
			// Simple "voices" command - show all voices
			DisplayAvailableVoices();
		}
		else if (parts.Length == 2)
		{
			// "voices <prefix>" command - filter by prefix
			var prefix = parts[1].Trim();
			if (string.IsNullOrEmpty(prefix))
			{
				Console.WriteLine("❌ Please specify a valid prefix.");
				Console.WriteLine("💡 Example: voices bf_");
				return;
			}

			DisplayAvailableVoices(prefix);
		}
		else
		{
			Console.WriteLine("❌ Invalid voices command format.");
			Console.WriteLine("💡 Usage: 'voices' or 'voices <prefix>'");
			Console.WriteLine("💡 Example: voices bf_");
		}
	}

	private static string? ReadLineWithTabCompletion()
	{
		var input = new List<char>();
		var completions = new List<string>();
		var completionIndex = -1;
		var originalInput = "";

		while (true)
		{
			var keyInfo = Console.ReadKey(true);

			switch (keyInfo.Key)
			{
				case ConsoleKey.Enter:
					Console.WriteLine();
					return new string([.. input]);

				case ConsoleKey.Backspace:
					if (input.Count > 0)
					{
						input.RemoveAt(input.Count - 1);
						Console.Write("\b \b");
						// Reset completions when input changes
						completions.Clear();
						completionIndex = -1;
					}

					break;

				case ConsoleKey.Tab:
					var currentInput = new string([.. input]);

					// Handle tab completion for voice command
					if (currentInput.StartsWith("voice ", StringComparison.OrdinalIgnoreCase))
					{
						var prefix = currentInput[6..]; // Remove "voice " prefix

						if (completionIndex == -1)
						{
							// First tab press - generate completions
							originalInput = currentInput;
							completions = GetVoiceCompletions(prefix);

							if (completions.Count == 0)
							{
								// No completions available - just show a beep or continue
								Console.Beep();
								break;
							}

							completionIndex = 0;
						}
						else
						{
							// Subsequent tab presses - cycle through completions
							completionIndex = (completionIndex + 1) % completions.Count;
						}

						if (completions.Count > 0)
						{
							// Clear current input and replace with completion
							ClearCurrentLine(input.Count);
							var completion = $"voice {completions[completionIndex]}";
							Console.Write(completion);
							input.Clear();
							input.AddRange(completion);
						}
					}
					else
					{
						// Reset completion state for non-voice commands
						completions.Clear();
						completionIndex = -1;
					}

					break;

				case ConsoleKey.Escape:
					// Reset to original input if we were in completion mode
					if (completionIndex != -1 && !string.IsNullOrEmpty(originalInput))
					{
						ClearCurrentLine(input.Count);
						Console.Write(originalInput);
						input.Clear();
						input.AddRange(originalInput);
						completions.Clear();
						completionIndex = -1;
					}

					break;

				default:
					// Regular character input
					if (char.IsControl(keyInfo.KeyChar))
						break;

					input.Add(keyInfo.KeyChar);
					Console.Write(keyInfo.KeyChar);

					// Reset completions when input changes
					completions.Clear();
					completionIndex = -1;
					break;
			}
		}
	}

	private static List<string> GetVoiceCompletions(string prefix)
	{
		var voices = KokoroVoiceManager.Voices.ToList();
		var prefixLower = prefix.ToLowerInvariant();

		return [.. voices
			.Where(voice => voice.Name.ToLowerInvariant().StartsWith(prefixLower, StringComparison.Ordinal))
			.Select(voice => voice.Name)
			.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)];
	}

	private static void ClearCurrentLine(int length)
	{
		// Move cursor back to beginning of input
		for (int i = 0; i < length; i++)
		{
			Console.Write("\b");
		}

		// Clear the line by overwriting with spaces
		for (int i = 0; i < length; i++)
		{
			Console.Write(" ");
		}

		// Move cursor back to beginning again
		for (int i = 0; i < length; i++)
		{
			Console.Write("\b");
		}
	}
}
