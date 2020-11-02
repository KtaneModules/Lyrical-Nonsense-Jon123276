using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using Rnd = UnityEngine.Random;
public class LyricalNonsence : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public TextMesh[] Displays;
    public KMSelectable[] Buttons;
    int songSelect = 0;
    static int moduleIdCounter = 1;
    int moduleId;
    bool moduleSolved;
    bool idle1 = true;
    bool start = false;
    int time = 0;
    string[] defaultDisplay = { "Song is: ", "Playing" };
    Songs selectedSong;

    struct Songs
    {
        public string Band;
        public string Song;
        public string[] SongParts;
    }

    List<Songs> songs = new List<Songs>()
    {
        new Songs { Band = "Rings of Saturn", Song = "The Husk", SongParts = new string[] {"The Husk n1", "The Husk n2", "The Husk n3" } },
        new Songs { Band = "Slaughter to Prevail", Song = "Demolisher", SongParts = new string[] { "Demolisher n1", "Demolisher n2", "Demolisher n3" } },
        new Songs { Band = "Fit For An Autopsy", Song = "Mirrors", SongParts = new string[] { "Mirrors n1", "Mirrors n2", "Mirrors n3" } },
        new Songs { Band = "Parkway Drive", Song = "The Void", SongParts = new string[] { "The Void n1", "The Void n2", "The Void n3" } },
        new Songs { Band = "Arch Enemy", Song = "The eagle flies alone", SongParts = new string[] {"Eagle n1", "Eagle n2", "Eagle n3"}},
        new Songs { Band = "Archspire", Song = "Involuntary Doppelgänger", SongParts = new string[] {"Doppel n1", "Doppel n2", "Doppel n3"}},
        new Songs { Band = "Frowning", Song = "In Solitude", SongParts = new string[] {"Solitude n1", "Solitude n2", "Solitude n3"}},
        new Songs { Band = "Spawn of Possession", Song = "Apparition", SongParts = new string[] { "Apparition n1", "Apparition n2", "Apparition n3"}},
        new Songs { Band = "Fleshgod Apocalypse", Song = "The Violation", SongParts = new string[] { "The n1", "The n2", "The n3"}},
        new Songs { Band = "Inferi", Song = "Those who from the Heaven Came", SongParts = new string[] { "Came n1", "Came n2", "Came n3"}},
        new Songs { Band = "Hideous Divinity", Song = "Ages Die", SongParts = new string[] { "Die n1", "Die n2", "Die n3"}},
        new Songs { Band = "Vektor", Song = "Fast Paced Society", SongParts = new string[] { "Vektor n1", "Vektor n2", "Vektor n3"}},
        new Songs { Band = "The Faceless", Song = "Hymn of Sanity", SongParts = new string[] { "Sanity n1", "Sanity n2", "Sanity n3"}},
        new Songs { Band = "Whispered", Song = "Exile of the Floating World", SongParts = new string[] { "Exile n1", "Exile n2", "Exile n3"}},        
        new Songs { Band = "Exilelord", Song = "Monochrome", SongParts = new string[] { "Mono n1", "Mono n2", "Mono n3"}},
        new Songs { Band = "Imperial Circus Dead Decadence", Song = "Yomi yori Kikoyu, Koukoku No Hi To Honoo No Syoujo", SongParts = new string[] { "Yomi n1", "Yomi n2", "Yomi n3"}},
        new Songs { Band = "Irreversible Mechanism", Song = "Outburst", SongParts = new string[] { "Out n1", "Out n2", "Out n3"}},
        new Songs { Band = "Frontierer", Song = "The Collapse", SongParts = new string[] { "Collapse n1", "Collapse n2", "Collapse n3"}},
        
    };

    void Start()
    {
        moduleId = moduleIdCounter++;
        int u = Rnd.Range(0, songs.Count);
        selectedSong = songs[u];
        Debug.LogFormat("[Lyrical Nonsense #{0}], The song selected is {1}", moduleId, selectedSong.Song);
        Buttons[0].OnInteract += delegate
        {
            if (idle1)
            {
                Displays[0].text = "";
                Displays[1].text = "";
                StartCoroutine(ChangeText1());
                Audio.PlaySoundAtTransform(selectedSong.SongParts[Rnd.Range(0, 3)], Buttons[0].transform);
                idle1 = false;
            }
            else
            {
                songSelect++;
                songSelect %= 18;
                Displays[0].text = "";
                Displays[1].text = "";
                StartCoroutine(ChangeSongs(songSelect));
            }
            return false;
        };
        Buttons[1].OnInteract += delegate
        {
            if (idle1)
                return false;
            Displays[0].text = "";
            Displays[1].text = "";
            StartCoroutine(ChangeText1());
            Audio.PlaySoundAtTransform(selectedSong.SongParts[Rnd.Range(0, 3)], Buttons[0].transform);
            return false;
        };
        Buttons[2].OnInteract += delegate
        {
            Submitting(); return false;
        };
    }
    void Submitting()
    {
        if (selectedSong.Song.Equals(Displays[0].text) && selectedSong.Band.Equals(Displays[1].text))
        {
            moduleSolved = true;
            Displays[0].text = "Good job!";
            Displays[1].text = "You rock!";
            Module.OnPass();
            foreach(var Button in Buttons)
            {
                Button.gameObject.SetActive(false);
            }
        }
        else
        {
            Module.OnStrike();
        }
    }
    IEnumerator ChangeText1()
    {
        yield return null;
        for (int i = 0; i < defaultDisplay[0].Length; i++)
        {
            Displays[0].text += defaultDisplay[0][i];
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < defaultDisplay[1].Length; i++)
        {
            Displays[1].text += defaultDisplay[1][i];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
        Displays[0].text = "";
        Displays[1].text = "";
        songSelect = 0;
        for (int i = 0; i < "The Husk".Length; i++)
        {
            Displays[0].text += "The Husk"[i];
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < "Rings of Saturn".Length; i++)
        {
            Displays[1].text += "Rings of Saturn"[i];
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ChangeSongs(int a)
    {
        yield return null;
        switch (a)
        {
            case 0:
                for (int i = 0; i < "The Husk".Length; i++)
                {
                    Displays[0].text += "The Husk"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Rings of Saturn".Length; i++)
                {
                    Displays[1].text += "Rings of Saturn"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 1:
                for (int i = 0; i < "Demolisher".Length; i++)
                {
                    Displays[0].text += "Demolisher"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Slaughter to Prevail".Length; i++)
                {
                    Displays[1].text += "Slaughter to Prevail"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 2:
                for (int i = 0; i < "Mirrors".Length; i++)
                {
                    Displays[0].text += "Mirrors"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Fit For An Autopsy".Length; i++)
                {
                    Displays[1].text += "Fit For An Autopsy"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 3:
                for (int i = 0; i < "The Void".Length; i++)
                {
                    Displays[0].text += "The Void"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Parkway Drive".Length; i++)
                {
                    Displays[1].text += "Parkway Drive"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 4:
                for (int i = 0; i < "The eagle flies alone".Length; i++)
                {
                    Displays[0].text += "The eagle flies alone"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Arch Enemy".Length; i++)
                {
                    Displays[1].text += "Arch Enemy"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 5:
                for (int i = 0; i < "Involuntary Doppelgänger".Length; i++)
                {
                    Displays[0].text += "Involuntary Doppelgänger"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Archspire".Length; i++)
                {
                    Displays[1].text += "Archspire"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 6:
                for (int i = 0; i < "In Solitude".Length; i++)
                {
                    Displays[0].text += "In Solitude"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Frowning".Length; i++)
                {
                    Displays[1].text += "Frowning"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 7:
                for (int i = 0; i < "Apparition".Length; i++)
                {
                    Displays[0].text += "Apparition"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Spawn of Possession".Length; i++)
                {
                    Displays[1].text += "Spawn of Possession"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 8:
                for (int i = 0; i < "The Violation".Length; i++)
                {
                    Displays[0].text += "The Violation"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Fleshgod Apocalypse".Length; i++)
                {
                    Displays[1].text += "Fleshgod Apocalypse"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 9:
                Displays[0].fontSize = 75;
                for (int i = 0; i < "Those who from the Heaven Came".Length; i++)
                {
                    Displays[0].text += "Those who from the Heaven Came"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Inferi".Length; i++)
                {
                    Displays[1].text += "Inferi"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 10:
                Displays[0].fontSize = 90;
                for (int i = 0; i < "Ages Die".Length; i++)
                {
                    Displays[0].text += "Ages Die"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Hideous Divinity".Length; i++)
                {
                    Displays[1].text += "Hideous Divinity"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 11:
                for (int i = 0; i < "Fast Paced Society".Length; i++)
                {
                    Displays[0].text += "Fast Paced Society"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Vektor".Length; i++)
                {
                    Displays[1].text += "Vektor"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 12:
                for (int i = 0; i < "Hymn of Sanity".Length; i++)
                {
                    Displays[0].text += "Hymn of Sanity"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "The Faceless".Length; i++)
                {
                    Displays[1].text += "The Faceless"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 13:
                for (int i = 0; i < "Exile of the Floating World".Length; i++)
                {
                    Displays[0].text += "Exile of the Floating World"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Whispered".Length; i++)
                {
                    Displays[1].text += "Whispered"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 14:
                for (int i = 0; i < "Monochrome".Length; i++)
                {
                    Displays[0].text += "Monochrome"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Exilelord".Length; i++)
                {
                    Displays[1].text += "Exilelord"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 15:
                Displays[0].fontSize = 45;
                Displays[1].fontSize = 60;
                for (int i = 0; i < "Yomi yori Kikoyu, Koukoku No Hi To Honoo No Syoujo".Length; i++)
                {   
                    Displays[0].text += "Yomi yori Kikoyu, Koukoku No Hi To Honoo No Syoujo"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Imperial Circus Dead Decadence".Length; i++)
                {
                    Displays[1].text += "Imperial Circus Dead Decadence"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 16:
                Displays[0].fontSize = 90;
                Displays[1].fontSize = 80;
                for (int i = 0; i < "Outburst".Length; i++)
                {
                    Displays[0].text += "Outburst"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Irreversible Mechanism".Length; i++)
                {
                    Displays[1].text += "Irreversible Mechanism"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 17:
                Displays[1].fontSize = 90;
                for (int i = 0; i < "The Collapse".Length; i++)
                {
                    Displays[0].text += "The Collapse"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                for (int i = 0; i < "Frontierer".Length; i++)
                {
                    Displays[1].text += "Frontierer"[i];
                    yield return new WaitForSeconds(0.01f);
                }
                break;
        }
    }
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} start (to start the song), !{0} next (to cycle to the next song in the display, !{0} again (to play the same song again), !{0} submit (to submit the current display's.)";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        Match m;

        if ((m = Regex.Match(command, @"^\s*(start|next)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success)
        {
            
            var button = m.Groups[1].Value;
            if (start) { yield return "sendtochaterror Module already started."; yield break; }
            if (button.Equals("start"))
            {
                yield return null;
                Buttons[0].OnInteract();
            }
            else
            {
                yield return null;
                Buttons[0].OnInteract();
            }
        }
        else if (Regex.IsMatch(command, @"^\s*again\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Buttons[1].OnInteract();
        }
        else if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Buttons[2].OnInteract();
        }
        else
        {
            yield return "sendtochaterror You have tried entering an invalid command, please use !# help to find the right commands!";
            yield break;
        }
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        Displays[0].text = selectedSong.Song;
        Displays[1].text = selectedSong.Band;
        Buttons[2].OnInteract();
    }
}
