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

    IEnumerator textChange;
    IEnumerator songCycle;

    void Start()
    {
        moduleId = moduleIdCounter++;
        int u = Rnd.Range(0, songs.Count);
        selectedSong = songs[u];
        Debug.LogFormat("[Lyrical Nonsense #{0}], The song selected is {1}", moduleId, selectedSong.Song);
        Buttons[0].OnInteract += delegate
        {
            if (textChange != null) StopCoroutine(textChange);
            if (songCycle != null) StopCoroutine(songCycle);
            if (idle1)
            {
                Displays[0].text = "";
                Displays[1].text = "";
                textChange = ChangeText1();
                StartCoroutine(textChange);
                Audio.PlaySoundAtTransform(selectedSong.SongParts[Rnd.Range(0, 3)], Buttons[0].transform);
                idle1 = false;
            }
            else
            {
                songSelect++;
                songSelect %= 18;
                Displays[0].text = "";
                Displays[1].text = "";
                songCycle = ChangeSongs(songSelect);
                StartCoroutine(songCycle);
            }
            return false;
        };
        Buttons[1].OnInteract += delegate
        {
            if (idle1)
                return false;
            if (textChange != null) StopCoroutine(textChange);
            if (songCycle != null) StopCoroutine(songCycle);
            Displays[0].text = "";
            Displays[1].text = "";
            textChange = ChangeText1();
            StartCoroutine(textChange);
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
        Displays[0].fontSize = 90;
        Displays[1].fontSize = 90;
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
        string song = songs[a].Song;
        if (a == 9)
        {
            Displays[0].fontSize = 75;
        }else if (a == 15)
        {
            Displays[0].fontSize = 45;
            Displays[1].fontSize = 60;
        }else if(a == 16)
        {
            Displays[0].fontSize = 90;
            Displays[1].fontSize = 80;
        }else
        {
            Displays[0].fontSize = 90;
            Displays[1].fontSize = 90;
        }
        for (int i = 0; i < song.Length; i++)
        {
            Displays[0].text += song[i];
            yield return new WaitForSeconds(0.01f);
        }
        string band = songs[a].Band;
        for (int i = 0; i < band.Length; i++)
        {
            Displays[1].text += band[i];
            yield return new WaitForSeconds(0.01f);
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
