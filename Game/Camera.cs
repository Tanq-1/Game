using System;

namespace Game
{
    public class Camera
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private int viewportWidth;
        private int viewportHeight;
        private (int xMin, int xMax, int yMin, int yMax) _cameraArgs;

        public Camera(int viewportWidth, int viewportHeight, (int xMin, int xMax, int yMin, int yMax) _cameraArgs)
        {
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
            this._cameraArgs = _cameraArgs;
        }

        public void Follow(int targetX, int targetY)
        {
            if (Methods.Scene == "Outside")
            {
                _cameraArgs = Methods.WorldLimit;
            }
            else
            {
                _cameraArgs = Methods.HouseLimit;
            }
                X = targetX - viewportWidth / 2;
            Y = targetY - viewportHeight / 2;

            // Clamp to world bounds
            X = Math.Clamp(X, _cameraArgs.xMin, _cameraArgs.xMax - viewportWidth);
            Y = Math.Clamp(Y, _cameraArgs.yMin, _cameraArgs.yMax - viewportHeight);
        }
        public void CameraStop()
        {
            X = 0;
            Y = 0;
        }
    }
}
