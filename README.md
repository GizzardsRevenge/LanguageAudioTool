# LanguageAudioTool 2020 GizzardsRevenge 

This is a tool designed for batch editing language-learning MP3 files. It might be useful for other, more general, audio file batch editing.
It's very rough-and-ready, but thought it might already be in a stage where it might be useful for other people.

1. INTRODUCTION
2. USAGE
3. TODO
4. NOTES
5. CONTACT

1. INTRODUCTION ================================================================================

Many language learning courses contain (or comprise of) a number of audio files for practicing listening.
Quite a lot of these are in what I'll call "exam format": some audio played once at full, fluent-speaker speed, with silences of 15 seconds or more between
the audio sections.
This isn't very useful for listening practice, in my experience, as its pretty hard to learn new words or grammar patterns while trying to parse full-speed speech, particularly if it's
played only once.
And the long silence between sections is just time for my mind to wander.

This app is aimed at making files like this more useful. You give it a bunch of audio files, and a set of instructions.
Then, for each audio file, for each contiguous audio section, it will perform those actions. 

So, let's say you have a file "EasyPeasyFrench_1.mp3", and it contains 5 dialogues, and the first dialogue is like this:

00:00 - 00:05 <5 secs silence>
00:05 - 01:05 Audio dialogue: "Pierre learns basketball"
01:05 - 01:25 <20 secs silence>
... <next dialogues...>

If you have configured LanguageAudioTool as the setup: "Play at 80% speed, play 2 seconds of silence, play at 100% speed, play 2 seconds of silence", then in the resulting
output file, the first dialogue will be like this:

00:00 - 01:15 Audio dialogue: "Pierre learns basketball" played at 80% speed
01:15 - 01:17 <2 secs silence>
01:17 - 02:17 Audio dialogue: "Pierre learns basketball" played at 100% speed
02:15 - 02:17 <2 secs silence>
... <next dialogues...>

..and it will do the same thing for all of the dialogues of all of the files. 
So you notice it automatically strips all the silences, and just adds back the short silences that you configure it to.

This project makes use of:

NAudio library
NAudio Soundtouch library
NAudio Lame MP3 encoder

2. USAGE ================================================================================

It's basically like a wizard with 2 pages.

1. On the first page, you need to make a list of the files to edit. You can do this by either clicking "Add Files" or by dragging and dropping on to the main window. 
   For now, this app can only handle MP3 files. Then click next to go to the next window.
   
2. Here you make the list of instructions. The top window is the list of instructions and the two Add buttons are for adding either Audio or silence sections (as labelled). 
   The last instruction should be to add a silence, even of just 1 second, otherwise audio sections will run right into each other, which will sound weird.

3. You can also decide what to do with the first audio section. Since it's common for language audio files to have some copyright notice or whatever at the start, you can choose
   whether the first section should be treated as a normal audio section (i.e. perform the list of actions on it), just play it once at 100% speed, or remove it altogether.
   
4. When you hit Go, it will ask you where you want to save the files. As a precaution, given that this app may potentially break files (the underlying logic is fairly simple, I make
   no guarantees), it will not let you save over existing files. You need to pick a folder that does not contain any of the source files.
   
5. It takes a minute or two to process large files. It's quicker on Windows 8 or higher OS, because these versions have a built in MP3 encoder in Windows Media Foundation.
   For Windows 7 or lower, I use the lame MP3 encoder...which, despite the name, is no slower that the Microsoft encoder, however, it's slower *for this app*, because it  
   requires an extra conversion step.
   
3. TODO ================================================================================

1. Convert other audio formats than MP3
2. More gracefully handle certain error cases (e.g. if a file doesn't contain any audio at all; right now it will just blow up)
3. Include audio *player* functionality, where you can listen to an mp3, and it can perform your desired actions in real time
4. Add another kind of instruction: Beeps. Beeps might make for a better divider than silences.
5. Auto volume scaling. Scale the volume of files such that their loudest point will be at 100% volume (often language audio files are too quiet)

4. NOTES ================================================================================

Detecting silence proved to be a far harder task than expected.
You might think that you would just set some threshold volume and if all the samples are below that level, it's silence.

But that doesn't work; even an audio file that has had noise removed often will have compression artefacts; even in a part of the audio that will have no audible sound, and will 
display as a flat line on an amplitude graph will actually have a surprisingly high number of "noisy" samples.
The algorithm therefore includes thresholds not just for volume level but also for number of samples that need to be above that threshold, per 0.2 seconds, to count as being an audio 
section. It then joins up adjacent sections that have the same type.

A consequence of this algorithm is that it might erroneously cut up to 0.2 seconds of audio at the beginning or end of a section. Or, more likely, have up to 0.2s of padding at the 
beginning or end. This 0.2 value can be changed in the code, but making it smaller may affect performance (or even accuracy at extreme small durations).

I've tested this algorithm on around 100 audio files for 4-5 different sources. However, it would not shock me if it failed to parse any particular file incorrectly, hence why I don't
let it write over existing mp3 files. If you find an example of a file it fails at, then please email me.

5. CONTACT ==============================================================================

Stephen Holtom
stephen.holtom@gmail.com



