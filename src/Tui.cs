﻿using gart.Algorithms;

using NStack;

using Terminal.Gui;

namespace gart;

public class Tui
{
    public void Start()
    {
        var algorithms = new IAlgorithm[] { new SimpleBoxes(), new PixelSort() };

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

        var win = new Window("gart - The Generative Art Maker")
        {
            X = 0,
            Y = 1, // Leave one row for the toplevel menu

            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        top.Add(menu, win);

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
            Y = Pos.Bottom(algoListFrame)+1,
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
            Width = 30,
            Height = 1
        };
        outputOptFrame.Add(outputFile);
        var outputChooser = new Button("...")
        {
            X = Pos.Right(outputFile) + 1,
            Y = Pos.Top(label),
        };
        outputChooser.Clicked += () => ChooseFile();
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
        };

        algoDetails.Text = algorithms[algoListView.SelectedItem].Description;
        algoDetails.WordWrap = true;

        algoDetailsFrame.Add(algoDetails);

        win.Add(algoListFrame, algoDetailsFrame, outputOptFrame);

        algoListView.SetFocus();
        generateButton.Clicked += delegate () { GenerateButton_Clicked(outputWidth.Text, outputHeight.Text, outputFile.Text, algoListView.SelectedItem); };

        algoListView.SelectedItemChanged += AlgoListView_SelectedItemChanged;

        Application.Run();

        void ChooseFile()
        {
            var dialog = new FileDialog("Save art as...", "Ok", "File", "Choose where to save your generated art", allowedTypes: new List<string> { ".jpg" })
            {
                DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            };

            Application.Run(dialog);

            if (!dialog.Canceled)
            {
                outputFile.Text = dialog.FilePath;
            }
        }

        void AlgoListView_SelectedItemChanged(ListViewItemEventArgs args)
        {
            algoDetails.Text = algorithms[args.Item].Description;
        }
    }

    private void GenerateButton_Clicked(ustring text1, ustring text2, ustring text3, int selectedItem)
    {
        Console.WriteLine("Weee");
    }

}