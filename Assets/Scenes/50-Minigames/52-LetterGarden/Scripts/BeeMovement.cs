using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Scenes.Minigames.LetterGarden.Scripts
{
    public class BeeMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private GameObject SplineParent;

        public SplineContainer letterSpline;

        private Vector3 currentPos;
        private Vector3 direction;
        private float distancePercentage = 0;
        private float spineLeangth;
        public int splineIndex = 0;

        private readonly int difficultyEasy = 3;
        private int difficultyCurrent = 3;
        private int completedLetters = 0;

        /// <summary>
        /// Runs at start and dynamically fetches all splines used to draw letters/symbols, then selects one at random.
        /// </summary>
        private void Start()
        {
            SetDifficulty(difficultyEasy); //TODO: Placeholder until difficulty selection is created
            //SetDifficulty(difficultTest);
            SetLettersToDraw();
            NextLetter();
        }

        private void SetDifficulty(int difficulty)
        {
            difficultyCurrent = difficulty;
        }

        /// <summary>
        /// Finds all possible letters and assigns a number to be drawn depending on difficulty level.
        /// </summary>
        private void SetLettersToDraw()
        {
            foreach (Transform spline in SplineParent.GetComponentInChildren<Transform>())
            {
                letterList.Add(spline.gameObject.GetComponent<SplineContainer>());
            }
            if (difficultyCurrent > letterList.Count)
            {
                Debug.LogError("The difficulty level for LetterGarden is attempting to load more letters than is available.");
            }
            for (completedLetters = 0; completedLetters < difficultyCurrent; completedLetters++)
            {
                SplineContainer currentLetter = letterList[Random.Range(0, letterList.Count)];
                lettersToDraw.Add(currentLetter);
                letterList.Remove(currentLetter);
            }
        }

        /// <summary>
        /// Called to switch to the next letter, once the previous one has been completed.
        /// </summary>
        public void NextLetter(SplineContainer currentLetter)
        {
            letterSpline = currentLetter;
            splineIndex = 0;
            spineLeangth = letterSpline.CalculateLength(splineIndex);
        }

        private void Update()
        {
            if (letterSpline != null)
            {
                MoveOnSpline();
                CheckDistance();
            }
        }

        /// <summary>
        /// Checks how close to completion of the current path the bee is.
        /// TODO: Remove this once merged with the code to check if the previous line is done.
        /// </summary>
        private void CheckDistance()
        {
            if (distancePercentage >= 1.05)
            {
                distancePercentage = -0.05f;
            }
        }

        /// <summary>
        /// sends the bee to the next line of the letter.
        /// </summary>
        /// <returns>returns false if you cant go to the next line(we are out of lines) and returns true if it sucsesfully moves on to the next line</returns>
        public bool NextSplineInLetter()
        {
            bool result = splineIndex < letterSpline.Splines.Count - 1;
            if (result)
            {
                splineIndex++;
                spineLeangth = letterSpline.CalculateLength(splineIndex);
            }
            distancePercentage = 0f;
            return result;
        }

        /// <summary>
        /// call onece pr frame to move the bee on the current path.
        /// </summary>
        public void MoveOnSpline()
        {
            distancePercentage += speed * Time.deltaTime / spineLeangth;

            currentPos = letterSpline.EvaluatePosition(splineIndex, distancePercentage);
            transform.position = currentPos;

            direction = (Vector3)letterSpline.EvaluatePosition(splineIndex, distancePercentage + 0.05f) - currentPos;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.back);
            }
        }
    }
}