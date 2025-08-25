using UnityEngine;

public class SpecialMove2 : NewPlayMove
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void PlayerStart2()
    {

    }

    protected virtual void Awake()
    {

    }

   // public bool IsGrounded => _groundedPlayer;

    public void JumpIA()
    {
        if (_groundedPlayer && !_isCrouching)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);

        }
    }

    public void CrouchIA(bool crouch)
    {
        if (crouch && !_isCrouching)
        {
            _isCrouching = true;
            _controller.height = _crouchHeight;
            Vector3 center = _controller.center;
            center.y = _crouchHeight / 2f;
            _controller.center = center;
            transform.position += new Vector3(0, (_crouchHeight - _originalHeight) / 2f, 0);
        }
    }

    public void BlockIA(bool block)
    {
        _isBlocking = block;
    }
}
