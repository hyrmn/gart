using gart.Algorithms;

using System.ComponentModel;

using Terminal.Gui;

namespace gart;

public class Tui
{
    private static readonly IAlgorithm[] algorithms = new IAlgorithm[] { new SimpleBoxes(), new PixelSort(), new OverlappingDecahedrons(), new RandomGranite() };

    public void Start()
    {
        Application.Init();

        var top = Application.Top;

        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
            new MenuBarItem("_Help", new MenuItem[]
            {
                new MenuItem("_About", string.Empty, () =>
                {
                    MessageBox.Query("gart 0.1.0", "\ngart helps you generate art\n\nDeveloped by Ben Hyrman\nand licensed under The Unlicense.\n ", "Close");
                }),
            }),
        });

        var statusMessage = new StatusItem(Key.Null, "Initialized and ready...", null);
        var statusBar = new StatusBar(new StatusItem[] { statusMessage });

        var win = new Window("gart - The Generative Art Maker")
        {
            X = 0,
            Y = 1, // Leave one row for the toplevel menu

            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        top.Add(menu, win, statusBar);

        var algoListFrame = new FrameView("Choose a generator")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(70),
            Height = Dim.Percent(65),
        };

        var algoListView = new ListView()
        {
            X = 0,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            AllowsMarking = false,
            AllowsMultipleSelection = false
        };
        algoListView.SetSource(algorithms);

        algoListFrame.Add(algoListView);

        var algoListScroll = new ScrollBarView(algoListView, isVertical: true, showBothScrollIndicator: false);

        algoListScroll.ChangedPosition += () =>
        {
            algoListView.TopItem = algoListScroll.Position;
            if (algoListView.TopItem != algoListScroll.Position)
            {
                algoListScroll.Position = algoListView.TopItem;
            }
            algoListView.SetNeedsDisplay();
        };

        algoListView.DrawContent += (e) =>
        {
            algoListScroll.Size = algoListView.Source.Count - 1;
            algoListScroll.Position = algoListView.TopItem;
            algoListScroll.Refresh();
        };

        var outputOptFrame = new FrameView("Output Options")
        {
            X = 0,
            Y = Pos.Bottom(algoListFrame) + 1,
            Width = Dim.Percent(70),
            Height = Dim.Percent(35),
        };
        win.Add(outputOptFrame);

        var label = new Label("width:")
        {
            X = 0,
            Y = 0,
            Width = 15,
            Height = 1,
            TextAlignment = TextAlignment.Right,
        };
        outputOptFrame.Add(label);

        var outputWidth = new TextField("3440")
        {
            X = Pos.Right(label) + 1,
            Y = Pos.Top(label),
            Width = 5,
            Height = 1
        };
        outputOptFrame.Add(outputWidth);

        label = new Label("height:")
        {
            X = 0,
            Y = Pos.Bottom(label),
            Width = Dim.Width(label),
            Height = 1,
            TextAlignment = TextAlignment.Right,
        };
        outputOptFrame.Add(label);
        var outputHeight = new TextField("1440")
        {
            X = Pos.Right(label) + 1,
            Y = Pos.Top(label),
            Width = 5,
            Height = 1
        };
        outputOptFrame.Add(outputHeight);

        label = new Label("input:")
        {
            X = 0,
            Y = Pos.Bottom(label),
            Width = Dim.Width(label),
            Height = 1,
            TextAlignment = TextAlignment.Right,
        };
        outputOptFrame.Add(label);
        var inputFiles = new TextField("")
        {
            X = Pos.Right(label) + 1,
            Y = Pos.Top(label),
            Width = 40,
            Height = 1
        };
        outputOptFrame.Add(inputFiles);
        var inputChooser = new Button("...")
        {
            X = Pos.Right(inputFiles) + 1,
            Y = Pos.Top(label),
        };
        inputChooser.Clicked += () => ChooseInputFiles();
        outputOptFrame.Add(inputChooser);

        label = new Label("output:")
        {
            X = 0,
            Y = Pos.Bottom(label),
            Width = Dim.Width(label),
            Height = 1,
            TextAlignment = TextAlignment.Right,
        };
        outputOptFrame.Add(label);

        var outputFile = new TextField("")
        {
            X = Pos.Right(label) + 1,
            Y = Pos.Top(label),
            Width = 40,
            Height = 1
        };
        outputOptFrame.Add(outputFile);

        var outputChooser = new Button("...")
        {
            X = Pos.Right(outputFile) + 1,
            Y = Pos.Top(label),
        };
        outputChooser.Clicked += () => ChooseOutputFile();
        outputOptFrame.Add(outputChooser);

        var generateButton = new Button("Gen")
        {
            X = Pos.Right(outputFile) + 1,
            Y = Pos.Bottom(label) + 1,
        };
        outputOptFrame.Add(generateButton);

        var algoDetailsFrame = new FrameView("Algorithm Details")
        {
            X = Pos.Right(algoListFrame),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };

        var algoDetails = new TextView()
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true,
            CanFocus = false,
        };

        algoDetails.Text = selectedAlgorithm().Description;
        algoDetails.WordWrap = true;

        algoDetailsFrame.Add(algoDetails);

        win.Add(algoListFrame, algoDetailsFrame, outputOptFrame);

        algoListView.SetFocus();

        generateButton.Clicked += GenerateButton_Clicked;
        outputFile.KeyDown += OutputFile_KeyDown;
        algoListView.SelectedItemChanged += AlgoListView_SelectedItemChanged;

        updateInputFileControls();

        Application.Run();

        IAlgorithm selectedAlgorithm() => algorithms[algoListView.SelectedItem];

        void ChooseInputFiles()
        {
            if (selectedAlgorithm() is not IGenerateWithSource) return;

            bool allowsMultiple = ((IGenerateWithSource)selectedAlgorithm()).AllowMultipleFiles;

            var dialog = new OpenDialog("Choose input files", "Choose input file(s) for your generator", allowedTypes: new List<string> { ".jpg" })
            {
                AllowsMultipleSelection = allowsMultiple,
                DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            };

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                inputFiles.Text = dialog.FilePath;
                inputFiles.Data = dialog.FilePaths;
            }
        }

        void ChooseOutputFile()
        {
            var dialog = new FileDialog("Save art as...", "Ok", "File", "Choose where to save your generated art", allowedTypes: [".png"])
            {
                DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                outputFile.Text = dialog.FilePath;
            }
        }

        void GenerateButton_Clicked()
        {
            GenerateArt();
        }

        void OutputFile_KeyDown(View.KeyEventEventArgs obj)
        {
            if (obj.KeyEvent.Key == Key.Enter)
            {
                GenerateArt();
            }
        }

        void GenerateArt()
        {
            if (outputWidth.Text.IsEmpty || outputHeight.Text.IsEmpty || outputFile.Text.IsEmpty)
            {
                MessageBox.ErrorQuery("Cannot generate", "You must specify a heigh, width and output file", "Close");
                return;
            }

            if (selectedAlgorithm() is IGenerateWithSource && inputFiles.Text.IsEmpty)
            {
                MessageBox.ErrorQuery("Cannot generate", "This generator requires at least one input file", "Close");
                return;
            }

            statusMessage.Title = "Generating...";
            statusBar.SetNeedsDisplay();

            var width = int.Parse(outputWidth.Text.ToString());
            var height = int.Parse(outputHeight.Text.ToString());
            var outputSize = new Dimensions(width, height);
            var destination = new Destination(new FileInfo(outputFile.Text.ToString()));

            var worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
            worker.DoWork += (s, e) =>
            {

                if (selectedAlgorithm() is IGenerateWithoutSource target)
                {
                    target.Generate(outputSize, destination);
                }
                else
                {
                    var sourceFile = new FileInfo(inputFiles.Text.ToString());
                    if(!sourceFile.Exists)
                    {
                        MessageBox.ErrorQuery("Cannot generate", $"Source {sourceFile.FullName} does not exist", "Close");
                        e.Cancel = true;
                        return;
                    }

                    var sourceFiles = new FileInfo[]
                    {
                        sourceFile
                    };

                    var source = new Source(sourceFiles);

                    ((IGenerateWithSource)selectedAlgorithm()).Generate(source, outputSize, destination);
                }

            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    statusMessage.Title = $"Last run failed";
                }
                else
                {
                    statusMessage.Title = $"Last run saved to {destination.File.FullName}";
                }
                statusBar.SetNeedsDisplay();
            };

            worker.RunWorkerAsync();
        }

        void AlgoListView_SelectedItemChanged(ListViewItemEventArgs args)
        {
            algoDetails.Text = algorithms[args.Item].Description;
            updateInputFileControls();
        }

        void updateInputFileControls()
        {
            const string emptyMessage = "<not required for this generator>";
            if (selectedAlgorithm() is IGenerateWithoutSource)
            {
                inputFiles.Enabled = false;
                if (inputFiles.Text.IsEmpty)
                {
                    inputFiles.Text = emptyMessage;
                }

                inputChooser.Enabled = false;
            }
            else
            {
                inputFiles.Enabled = true;
                if (inputFiles.Text == emptyMessage)
                {
                    inputFiles.Text = "";
                }

                inputChooser.Enabled = true;
            }
        }
    }
}