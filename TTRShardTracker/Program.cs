namespace TTRShardReaders {

    class Program {

        // Actual Game Directory
        //private static readonly DirectoryInfo gameDirectory = new DirectoryInfo(@"D:\Games\Other\Disney\Toontown\Rewritten\Toontown Rewritten");
        //private static readonly DirectoryInfo logsDirectoryFull = new DirectoryInfo(gameDirectory.FullName + @"\logs");

        //private static readonly FileInfo districtFile = new FileInfo(gameDirectory.FullName + @"\district.txt");
        //private static readonly FileInfo districtFile2 = new FileInfo(gameDirectory.FullName + @"\district2.txt");

        private static readonly DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        private static readonly DirectoryInfo logsDirectory = new DirectoryInfo(currentDirectory.FullName + @"\logs");

        private static readonly FileInfo districtFile = new FileInfo(currentDirectory.FullName + @"\district.txt");
        private static readonly FileInfo districtFile2 = new FileInfo(currentDirectory.FullName + @"\district2.txt");
        
        private const string shardIndicator = "Entering shard";
        private const string searchString = ":OTPClientRepository: Entering shard ";

        static readonly Dictionary<int, string> Districtionary = new Dictionary<int, string> {
            { 5000, "Gulp Gulch" },
            { 5010, "Splashport" },
            { 5020, "Fizzlefield" },
            { 5030, "Whoosh Rapids" },
            { 5040, "Blam Canyon" },
            { 5050, "Hiccup Hills" },
            { 5060, "Splat Summit" },
            { 5070, "Thwackville" },
            { 5080, "Zoink Falls" },
            { 5090, "Kaboom Cliffs" },
            { 5100, "Bounceboro" },
            { 5110, "Boingbury" },
            { 5130, "Splatville" },
            { 5210, "Zapwood" }
        };
        static void Main() {

            Console.WriteLine("If you select any part inside of the command window, this program will freeze and not update.");
            Console.WriteLine("Deselect the inside of the command window to return it to normal.\n");

            Console.WriteLine("Check your game directory for 2 text files.");
            Console.WriteLine("'district.txt' just holds the district name.");
            Console.WriteLine("'district2.txt' holds the district name and prefixes it with 'District: '.\n");

            Console.WriteLine("=============================================================================================\n");

            while(true) {
                // Get the last log file by date modified.
                //var log = logsDirectoryFull.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                var log = logsDirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                using(var stream = File.Open(log.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)) {
                    using(var reader = new StreamReader(stream)) {
                        while(!reader.EndOfStream) {

                            // Read last line of file.
                            string? line = string.Empty;
                            string lastLine = string.Empty;
                            while((line = reader.ReadLine()) != null)
                                if(line.Contains(shardIndicator))
                                    lastLine = line;

                            string shardNumber = string.Empty;
                            // Obtain the shard number.
                            if(lastLine.Length != 0)
                                shardNumber = lastLine[(lastLine.IndexOf(":") + searchString.Length)..].Remove(4, 5);
                            else if(lastLine.Length == 0)
                                break;

                            // Shard converted to an integer.
                            int shardNumberInt = int.Parse(shardNumber);

                            // If the shard number has a corresponding district name.
                            if(Districtionary.ContainsKey(shardNumberInt)) {
                                // Get the district name using that shard number.
                                string districtName = Districtionary[shardNumberInt];

                                // Check to see if the file isn't there.
                                if(File.Exists(districtFile.FullName)) {
                                    // If it is, just write to the file.
                                    File.WriteAllText(districtFile.FullName, districtName);

                                    // If it isn't there.
                                } else if(!File.Exists(districtFile.FullName)) {
                                    // Create it.
                                    File.Create(districtFile.FullName);
                                    // Then write.
                                    File.WriteAllText(districtFile.FullName, districtName);
                                }

                                // Check to see if the file isn't there.
                                if(File.Exists(districtFile2.FullName)) {
                                    // If it is, just write to the file.
                                    File.WriteAllText(districtFile2.FullName, $"District: {districtName}");

                                    // If it isn't there.
                                } else if(!File.Exists(districtFile2.FullName)) {
                                    // Create it.
                                    File.Create(districtFile2.FullName);
                                    // Then write.
                                    File.WriteAllText(districtFile2.FullName, $"District: {districtName}");
                                }

                                // Clear the line before writing it.
                                ClearCurrentConsoleLine();
                                // Write it to console.
                                Console.Write($"{shardNumberInt} = {districtName}\r");

                                // Runs every 'x' seconds to make sure it stays up to date and makes sure that your CPU doesn't melt.
                                Thread.Sleep(TimeSpan.FromSeconds(1));
                            }
                        }
                    }
                }
            }
        }

        public static void ClearCurrentConsoleLine() {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}