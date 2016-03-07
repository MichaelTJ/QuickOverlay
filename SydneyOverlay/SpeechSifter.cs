using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace SydneyOverlay
{
    public class SpeechSifter
    {
        public SpeechRecognitionEngine sre;
        public string curWord = "";
        public string[] controlWords { get; set; }
        private bool running = false;
        private Grammar oldGram;
        public SpeechSifter() { 
            sre = new SpeechRecognitionEngine();

            sre.SetInputToDefaultAudioDevice();
            sre.RecognizeCompleted += 
                new EventHandler<RecognizeCompletedEventArgs>(RecognizeCompletedHandler);
            /*
            sre.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
             * */
        }

        public void updateChoices(string[] choices)
        {
            choices = addControls(choices);
            
            Choices c = new Choices(choices);
            GrammarBuilder gb = new GrammarBuilder(c);
            Grammar newGram = new Grammar(gb);
            sre.LoadGrammar(newGram);
            if (oldGram != null)
            {
                sre.UnloadGrammar(oldGram);
            }
            oldGram = newGram;
            
            if (!running)
            {
                StartVoiceCommands();
            }
        }
        private string[] addControls(string[] choices)
        {
            //Bring in the new choices
            //Convert to a list so that control words (Save, Discard...)
            //Can be added
            List<string> newChoices = choices.ToList();
            foreach (string s in controlWords)
            {
                newChoices.Add(s);
            }
            //return old choices + control words
            return newChoices.ToArray();

        }
        private void RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
        }
        /*
        public void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            curWord = e.Result.Text;
        }
        */
        public void StopVoiceCommands()
        {
            sre.RecognizeAsyncCancel();
            sre.RecognizeAsyncStop();
        }
        public void StartVoiceCommands()
        {
            sre.RecognizeAsync(RecognizeMode.Multiple);
            running = true;
        }

    }

}



