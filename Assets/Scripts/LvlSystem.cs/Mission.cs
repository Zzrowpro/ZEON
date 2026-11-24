// using System.Collections.Generic;
// public class Mission
// {
//     public Objective mainObjective;
//     public List<Objective> bonusObjectives = new List<Objective>();
//     public int pointsForMain = 100;
//     public int[] starThresholds = { 100, 200, 300 }; // 1, 2, 3 stars

//     Dictionary<Objective, ObjectiveProgress> progress = new Dictionary<Objective, ObjectiveProgress>();

//     // can the player leave?
//     public bool CanLeave() => mainObjective.IsComplete(progress[mainObjective]);

//     public int GetScore()
//     {
//         int score = 0;
//         if (CanLeave()) score += pointsForMain;

//         foreach (var b in bonusObjectives)
//             if (b.IsComplete(progress[b]))
//                 score += b.points;

//         return score;
//     }

//     public int GetStars()
//     {
//         if (!CanLeave()) return 0;

//         int score = GetScore();
//         int stars = 0;
//         foreach (int t in starThresholds)
//             if (score >= t) stars++;
//         return stars;
//     }

//     public void Init()
//     {
//         progress.Clear();
//         progress[mainObjective] = new ObjectiveProgress();
//         foreach (var b in bonusObjectives)
//             progress[b] = new ObjectiveProgress();
//     }

//     public void OnItemCollected(string itemId, int amount = 1)
//     {
//         foreach (var kv in progress)
//             if (kv.Key is CollectObjective c && c.itemId == itemId)
//                 kv.Value.count += amount;
//     }

//     public void OnEnemyKilled(string enemyId)
//     {
//         foreach (var kv in progress)
//             if (kv.Key is KillObjective k && k.enemyId == enemyId)
//                 kv.Value.count++;
//     }

// }