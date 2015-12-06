using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;

namespace SydneyOverlay
{
    class SpeechSifter
    {
        private SpeechRecognitionEngine sre;
        public string curWord = "";
        public SpeechSifter() { 
            sre = new SpeechRecognitionEngine();

            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        }

        public void updateChoices(string[] choices)
        {
            sre.RecognizeAsyncStop();
            Choices c = new Choices(choices);
            GrammarBuilder gb = new GrammarBuilder(c);
            Grammar g = new Grammar(gb);
            sre.UnloadAllGrammars();
            sre.LoadGrammar(g);
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            curWord = e.Result.Text;
        }


    }

}



