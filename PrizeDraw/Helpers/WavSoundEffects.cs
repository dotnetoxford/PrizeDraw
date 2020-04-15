using System.IO;
using System.Media;

namespace PrizeDraw.Helpers
{
    public class WavSoundEffects : ISoundEffects
    {
        private readonly SoundPlayer _tileChangeSoundPlayer;
        private readonly SoundPlayer _winnerSoundPlayer;

        private const string TileChangeWavFile = @"Media/TileChange.wav";
        private const string WinnerWavFile = @"Media/Winner.wav";

        public WavSoundEffects()
        {
            _tileChangeSoundPlayer = File.Exists(TileChangeWavFile) ? new SoundPlayer(TileChangeWavFile) : null;
            _winnerSoundPlayer = File.Exists(WinnerWavFile) ? new SoundPlayer(WinnerWavFile) : null;
        }

        public void PlayTileChangeSound() =>
            _tileChangeSoundPlayer?.Play();

        public void PlayWinnerSound() =>
            _winnerSoundPlayer?.Play();
    }
}