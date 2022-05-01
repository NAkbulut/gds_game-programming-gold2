using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private RoomFirstGenerator generator;

    [SerializeField] private Slider s_minRoomWidth;
    [SerializeField] private Slider s_minRoomHeight;
    [SerializeField] private Slider s_maxRoomWidth;
    [SerializeField] private Slider s_maxRoomHeight;
    [SerializeField] private Slider s_DungeonWidth;
    [SerializeField] private Slider s_DungeonHeight;
    [SerializeField] private Slider s_minRoomCount;
    [SerializeField] private Slider s_maxRoomCount;
    [SerializeField] private Slider s_minHallSize;
    [SerializeField] private Slider s_maxHallSize;

    [SerializeField] private Text t_minRoomWidth;
    [SerializeField] private Text t_minRoomHeight;
    [SerializeField] private Text t_maxRoomWidth;
    [SerializeField] private Text t_maxRoomHeight;
    [SerializeField] private Text t_DungeonWidth;
    [SerializeField] private Text t_DungeonHeight;
    [SerializeField] private Text t_minRoomCount;
    [SerializeField] private Text t_maxRoomCount;
    [SerializeField] private Text t_minHallSize;
    [SerializeField] private Text t_maxHallSize;

    void Update()
    {
        t_minRoomWidth.text = s_minRoomWidth.value.ToString();
        t_minRoomHeight.text = s_minRoomHeight.value.ToString();
        t_maxRoomWidth.text = s_maxRoomWidth.value.ToString();
        t_maxRoomHeight.text = s_maxRoomHeight.value.ToString();
        t_DungeonWidth.text = s_DungeonWidth.value.ToString();
        t_DungeonHeight.text = s_DungeonHeight.value.ToString();
        t_minRoomCount.text = s_minRoomCount.value.ToString();
        t_maxRoomCount.text = s_maxRoomCount.value.ToString();
        t_minHallSize.text = s_minHallSize.value.ToString();
        t_maxHallSize.text = s_maxHallSize.value.ToString();

        generator.minRoomWidth = (int)s_minRoomWidth.value;
        generator.minRoomHeight = (int)s_minRoomHeight.value;
        generator.maxRoomWidth = (int)s_maxRoomWidth.value;
        generator.maxRoomHeight = (int)s_maxRoomHeight.value;
        generator.dungeonWidth = (int)s_DungeonWidth.value;
        generator.dungeonHeight = (int)s_DungeonHeight.value;
        generator.minRoomCount = (int)s_minRoomCount.value;
        generator.maxRoomCount = (int)s_maxRoomCount.value;
        generator.minHallSize = (int)s_minHallSize.value;
        generator.maxHallSize = (int)s_maxHallSize.value;
    }
}
