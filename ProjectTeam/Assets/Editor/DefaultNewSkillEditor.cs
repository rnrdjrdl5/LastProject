using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// 에디터 연습. 

/*
[CustomEditor(typeof(DefaultNewSkill))]
public class DefaultNewSkillEditor : Editor {


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DefaultNewSkill script = (DefaultNewSkill)target;
        
        for (int i = 0; i < script.PlayerSkillDebuffs.Count; i++)
        {
            script.PlayerSkillDebuffs[i].SetSkillDebuffType(
                (DefaultPlayerSkillDebuff.EnumSkillDebuff)EditorGUILayout.EnumPopup("Debuff " + (i + 1), script.PlayerSkillDebuffs[i].GetSkillDebuffType())
                );


            script.PlayerSkillDebuffs[i].SetMaxTime(
                    EditorGUILayout.FloatField(" - 최대시간", script.PlayerSkillDebuffs[i].GetMaxTime()));

            Debug.Log(script.PlayerSkillDebuffs[i].GetMaxTime());

            if (script.PlayerSkillDebuffs[i].GetSkillDebuffType() == DefaultPlayerSkillDebuff.EnumSkillDebuff.STUN)
            {

            }
                


        }
    }

}
*/