using System.Media;

namespace PrizeDraw.Helpers
{
    public class WavSoundEffects : ISoundEffects
    {
        private static readonly SoundPlayer _tileChangeSoundPlayer = new SoundPlayer(@"Media/TileChange.wav");
        private static readonly SoundPlayer _winnerSoundPlayer = new SoundPlayer(@"Media/Winner.wav");

        public void PlayTileChangeSound()
        {
            _tileChangeSoundPlayer.Play();
        }

        public void PlayWinnerSound()
        {
            _winnerSoundPlayer.Play();
        }
    }
}