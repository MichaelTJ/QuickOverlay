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
        private bool started = false;
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
            if (!started)
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
                started = true;
            }
        }
        private string[] addControls(string[] choices)
        {
            List<string> newChoices = choices.ToList();
            foreach (string s in controlWords)
            {
                newChoices.Add(s);
            }
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

    }

}



