using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController _controller;
    public float _runSpeed = 40f;
    float mHorizontalMove = 0f;
    bool mJump = false;

    // Update is called once per frame
    void Update()
    {
        mHorizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;
        if (Input.GetButtonDown("Jump"))
            mJump = true;
        //_controller.Move(mHorizontalMove * Time.fixedDeltaTime, false, mJump);

    }

    private void FixedUpdate()
    {
      // _controller.Move(mHorizontalMove * Time.fixedDeltaTime, false, mJump);
        mJump = false;
    }
}
