﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject {

	public string CharacterName = "New Name";
	public GameObject CharacterModel;
	public int PlayerNumber;
}