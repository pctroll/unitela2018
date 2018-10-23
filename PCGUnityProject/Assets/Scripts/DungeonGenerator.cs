using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Dungeon dungeon;
    public DungeonVisualizer visualizer;

    public void Show()
    {
        dungeon.Build();
        visualizer.Render(dungeon.grid);
    }

    private void Start()
    {
        if (dungeon == null || visualizer == null)
            Debug.LogError("Dungeon or Visualizer not set up");
        
        // Show();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Show();
        }
    }
}