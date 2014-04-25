using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Xml;

namespace TANYA
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer TANYA = new SpeechSynthesizer();
        string QEvent;
        string ProcWindow;
        double timer = 10;
        int count = 1;
        int commandCounter = 0;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"C:\Users\Nick\Documents\Commands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
   
        void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
         
        
            int ranNum = rnd.Next(1, 10);
            string speech = e.Result.Text;
            Console.Write("command number: " + commandCounter);
            commandCounter++;
            Commands(speech, ranNum);
            
        }

        public void Commands(string speech, int ranNum)
        {

            switch (commandCounter)
            {
                case 1:
                    {
                        // initial conversation
                        // open browser could be in this list

                        /* for example:
                         * speech.contains "weather"
                         * TANYA.speak("its sunny, look outside");
                         * reset commandCounter
                         * 
                            
                         */
                        break;
                    }
                case 2:
                    {
                        // mid point conversation
                        // 
                        break;
                    }
                case 3:
                    {
                        // closer conversation
                        break;
                    }
                default: commandCounter = 0;
                    break;
            }


            Console.Write(speech);
            switch (speech)
            {
                //GREETINGS
                case "hello":
                case "hello tanya":
                    if (ranNum < 6) { TANYA.Speak("Hello sir"); }
                    else if (ranNum > 5) { TANYA.Speak("Hi"); }
                    break;
                case "goodbye":
                case "goodbye tanya":
                case "close":
                case "close tanya":
                    TANYA.Speak("Until next time");
                    Close();
                    break;
                case "tanya":
                    if (ranNum < 5) { QEvent = ""; TANYA.Speak("Yes sir"); }
                    else if (ranNum > 4) { QEvent = ""; TANYA.Speak("Yes?"); }
                    break;

                //WEBSITES
                case "open website":
                    System.Diagnostics.Process.Start("url");
                    break;
                case "start firefox":
                    System.Diagnostics.Process.Start("firefox.exe");
                    
                    TANYA.Speak("firefox has opened, where do you want to go?");
                    break;
                case "go to facebook":
                    TANYA.Speak("headed to facebook");
                    break;
                //SHELL COMMANDS
                case "open program":
                    System.Diagnostics.Process.Start("file location");
                    TANYA.Speak("Loading");
                    break;

                //CLOSE PROGRAMS
                case "close program":
                    ProcWindow = "process name";
                    StopWindow();
                    break;

                //CONDITION OF DAY
                case "what time is it":
                    DateTime now = DateTime.Now;
                    string time = now.GetDateTimeFormats('t')[0];
                    TANYA.Speak(time);
                    break;
                case "what day is it":
                    TANYA.Speak(DateTime.Today.ToString("dddd"));
                    break;
                case "whats the date":
                case "whats todays date":
                    TANYA.Speak(DateTime.Today.ToString("dd-MM-yyyy"));
                    break;

                //OTHER COMMANDS
                case "go fullscreen":
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;
                    TANYA.Speak("expanding");
                    break;
                case "exit fullscreen":
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                    break;
                case "switch window":
                    SendKeys.Send("%{TAB " + count + "}");
                    count += 1;
                    break;
                case "reset":
                    count = 1;
                    timer = 11;
                    lblTimer.Visible = false;
                    ShutdownTimer.Enabled = false;
                    lstCommands.Visible = false;
                    break;
                case "out of the way":
                    if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Minimized;
                        TANYA.Speak("My apologies");
                    }
                    break;
                case "come back":
                    if (WindowState == FormWindowState.Minimized)
                    {
                        TANYA.Speak("Alright?");
                        WindowState = FormWindowState.Normal;
                    }
                    break;
                case "show commands":
                    string[] commands = (File.ReadAllLines(@"C:\Users\Nick\Documents\Commands.txt"));
                    TANYA.Speak("Very well");
                    lstCommands.Items.Clear();
                    lstCommands.SelectionMode = SelectionMode.None;
                    lstCommands.Visible = true;
                    foreach (string command in commands)
                    {
                        lstCommands.Items.Add(command);
                    }
                    break;
                case "hide listbox":
                    lstCommands.Visible = false;
                    break;

                //SHUTDOWN RESTART LOG OFF
                case "shutdown":
                    if (ShutdownTimer.Enabled == false)
                    {
                        QEvent = "shutdown";
                        TANYA.Speak("I will shutdown shortly");
                        lblTimer.Visible = true;
                        ShutdownTimer.Enabled = true;
                    }
                    break;
                case "log off":
                    if (ShutdownTimer.Enabled == false)
                    {
                        QEvent = "logoff";
                        TANYA.Speak("Logging off");
                        lblTimer.Visible = true;
                        ShutdownTimer.Enabled = true;
                    }
                    break;
                case "restart":
                    if (ShutdownTimer.Enabled == false)
                    {
                        QEvent = "restart";
                        TANYA.Speak("I'll be back shortly");
                        lblTimer.Visible = true;
                        ShutdownTimer.Enabled = true;
                    }
                    break;
                case "abort":
                    if (ShutdownTimer.Enabled == true)
                    {
                        QEvent = "abort";
                    }
                    break;
                case "speed up":
                    if (ShutdownTimer.Enabled == true)
                    {
                        ShutdownTimer.Interval = ShutdownTimer.Interval / 10;
                    }
                    break;
                case "slow down":
                    if (ShutdownTimer.Enabled == true)
                    {
                        ShutdownTimer.Interval = ShutdownTimer.Interval * 10;
                    }
                    break;
                default:
                    {
                    TANYA.Speak("What was that?");
                    break;
                    }
                }
        }
   
        private void ComputerTermination()
        {
            switch (QEvent)
            {
                case "shutdown":
                    System.Diagnostics.Process.Start("shutdown", "-s");
                    break;
                case "logoff":
                    System.Diagnostics.Process.Start("shutdown", "-l");
                    break;
                case "restart":
                    System.Diagnostics.Process.Start("shutdown", "-r");
                    break;
            }
        }
        private void StopWindow()
        {
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(ProcWindow);
            foreach (System.Diagnostics.Process proc in procs)
            {
                proc.CloseMainWindow();
            }
        }

 

        private void ShutdownTimer_Tick(object sender, EventArgs e)
        {
            if (timer == 0)
            {
                lblTimer.Visible = false;
                ComputerTermination();
                ShutdownTimer.Enabled = false;
            }
            else if (QEvent == "abort")
            {
                timer = 10;
                lblTimer.Visible = false;
                ShutdownTimer.Enabled = false;
            }
            else
            {
                timer = timer - .01;
                lblTimer.Text = timer.ToString();
            }
        } 
    }
}
