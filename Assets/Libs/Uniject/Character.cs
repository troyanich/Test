using UnityEngine;
using System.Collections;
using Uniject;
using Uniject.Impl;
using Collision = Uniject.Collision;

public class Character : TestableComponent
{
    public TestableGameObject obj { get; private set; }
    public IBoxCollider collider { get; private set; }
    public IRigidBody body { get; private set; }

    public Character(TestableGameObject obj,
                  IRigidBody body,
                  IBoxCollider collider
                  /*[Resource("Characters/char")] TestableGameObject character*/
                    ) : base(obj)
    {
        this.obj = obj;
        this.collider = collider;
        this.body = body;
        body.isKinematic = true;
        TestableGameObject character = new UnityGameObject(GameManager.instance.Character);
        character.transform.Parent = obj.transform;
        character.transform.LocalPosition = Vector3.zero;
        obj.transform.Position = GameManager.instance.LevelController.StartTransform.position;
        GameManager.instance.CanRun = true; //старт
    }

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (collision.gameObject.tag == "Coin")
        {
            collision.enabled = false;
            Object.Destroy(collision.gameObject);
            GameManager.instance.CoinsCount++;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            GameManager.instance.OnFinish();
        }
    }
}
