using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasCondition : Conditional
{
    [SerializeField] private CheckType check;
    [SerializeField] private int count = 1;

    private int c = 0;

    public override TaskStatus OnUpdate()
    {
        switch (check)
        {
            case CheckType.Time:
                c++;

                if (c >= count)
                {
                    c = 0;
                    return TaskStatus.Failure;
                }
                else return TaskStatus.Running;
            case CheckType.Forever:
                return TaskStatus.Running;
        }

        return TaskStatus.Failure;
    }
}
