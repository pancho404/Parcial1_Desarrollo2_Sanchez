using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FezManager : MonoBehaviour
{
    public enum FacingDirection
    {
        Front = 0,
        Right = 1,
        Back = 2,
        Left = 3
    }

    private Movement characterMovement;

    [SerializeField] private FacingDirection facingDirection;

    private FacingDirection lastFacingDirection;

    private GameObject player;

    public float degrees = 0;

    [SerializeField] private Transform levelData;

    [SerializeField] private Transform buildingData;

    [SerializeField] private GameObject invisiCube;

    [SerializeField] private GameObject playerCamera;

    private List<Transform> invisiList = new List<Transform>();

    private float lastDepth = 0f;

    [SerializeField] private float worldUnits = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        facingDirection = FacingDirection.Front;
        characterMovement = player.GetComponent<Movement>();
        UpdateLevelData(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterMovement.isJumping)
        {
            bool updateData = false;
            if (OnInvisiblePlatform())
            {
                if (MovePlayerDepthToClosestPlatform())
                {
                    updateData = true;
                }
            }
            if (MoveToClosestPlatformToCamera())
            {
                updateData = true;
            }
            if (updateData)
            {
                UpdateLevelData(false);
            }
        }
    }

    private void UpdateLevelData(bool forceRebuild)
    {
        //If facing direction and depth havent changed we do not need to rebuild
        if (!forceRebuild)
            if (lastFacingDirection == facingDirection && lastDepth == GetPlayerDepth())
                return;
        foreach (Transform tr in invisiList)
        {
            //Move obsolete invisicubes out of the way and delete

            tr.position = Vector3.zero;
            Destroy(tr.gameObject);

        }
        invisiList.Clear();
        float newDepth = 0f;

        newDepth = GetPlayerDepth();
        CreateInvisicubeAtNewDepth(newDepth);
    }

    private void OnRotateCameraRight()
    {
        if (OnInvisiblePlatform())
        {
            MovePlayerDepthToClosestPlatform();
        }
        lastFacingDirection = facingDirection;
        facingDirection = GetRotationRight();
        degrees -= 90f;
        UpdateLevelData(false);
        player.transform.Rotate(0, degrees, 0);
        player.transform.LookAt(playerCamera.transform);
    }

    private void OnRotateCameraLeft()
    {
        if (OnInvisiblePlatform())
        {
            MovePlayerDepthToClosestPlatform();
        }
        lastFacingDirection = facingDirection;
        facingDirection = GetRotationLeft();
        degrees += 90f;
        UpdateLevelData(false);
        player.transform.Rotate(0, degrees, 0);
        player.transform.LookAt(playerCamera.transform);
    }

    private bool OnInvisiblePlatform()
    {
        foreach (Transform transform in invisiList)
        {
            if (Mathf.Abs(transform.position.x - characterMovement.transform.position.x) < worldUnits && Mathf.Abs(transform.position.z - characterMovement.transform.position.z) < worldUnits)
            {
                if (characterMovement.transform.position.y - transform.position.y <= worldUnits + 0.2f && characterMovement.transform.position.y - transform.position.y > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool MoveToClosestPlatformToCamera()
    {
        bool moveCloser = false;
        foreach (Transform item in levelData)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - characterMovement.transform.position.x) < worldUnits + 0.1f)
                {

                    if (characterMovement.transform.position.y - item.position.y <= worldUnits + 0.2f && characterMovement.transform.position.y - item.position.y > 0 && !characterMovement.isJumping)
                    {
                        if (facingDirection == FacingDirection.Front && item.position.z < characterMovement.transform.position.z)
                        {
                            moveCloser = true;
                        }

                        if (facingDirection == FacingDirection.Back && item.position.z > characterMovement.transform.position.z)
                        {
                            moveCloser = true;
                        }


                        if (moveCloser)
                        {
                            characterMovement.transform.position = new Vector3(characterMovement.transform.position.x, characterMovement.transform.position.y, item.position.z);
                            return true;
                        }
                    }

                }

            }
            else
            {
                if (Mathf.Abs(item.position.z - characterMovement.transform.position.z) < worldUnits + 0.1f)
                {
                    if (characterMovement.transform.position.y - item.position.y <= worldUnits + 0.2f && characterMovement.transform.position.y - item.position.y > 0 && !characterMovement.isJumping)
                    {
                        if (facingDirection == FacingDirection.Right && item.position.x > characterMovement.transform.position.x)
                            moveCloser = true;

                        if (facingDirection == FacingDirection.Left && item.position.x < characterMovement.transform.position.x)
                            moveCloser = true;

                        if (moveCloser)
                        {
                            characterMovement.transform.position = new Vector3(item.position.x, characterMovement.transform.position.y, characterMovement.transform.position.z);
                            return true;
                        }

                    }

                }
            }


        }
        return false;
    }

    private bool FindTransformInvisiList(Vector3 cube)
    {
        foreach (Transform item in invisiList)
        {
            if (item.position == cube)
            {
                return true;
            }
        }
        return false;
    }

    private bool FindTransformLevel(Vector3 cube)
    {
        foreach (Transform item in levelData)
        {
            if (item.position == cube)
            {
                return true;
            }
        }
        return false;
    }

    private bool MovePlayerDepthToClosestPlatform()
    {
        foreach (Transform item in levelData)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                if (Mathf.Abs(item.position.x - characterMovement.transform.position.x) < worldUnits + 0.1f)
                    if (characterMovement.transform.position.y - item.position.y <= worldUnits + 0.2f && characterMovement.transform.position.y - item.position.y > 0)
                    {

                        characterMovement.transform.position = new Vector3(characterMovement.transform.position.x, characterMovement.transform.position.y, item.position.z);
                        return true;

                    }
            }
            else
            {
                if (Mathf.Abs(item.position.z - characterMovement.transform.position.z) < worldUnits + 0.1f)
                    if (characterMovement.transform.position.y - item.position.y <= worldUnits + 0.2f && characterMovement.transform.position.y - item.position.y > 0)
                    {

                        characterMovement.transform.position = new Vector3(item.position.x, characterMovement.transform.position.y, characterMovement.transform.position.z);
                        return true;
                    }
            }
        }
        return false;
    }

    private Transform CreateInvisicube(Vector3 position)
    {
        GameObject gameObject = Instantiate(invisiCube) as GameObject;

        gameObject.transform.position = position;

        return gameObject.transform;

    }

    private void CreateInvisicubeAtNewDepth(float newDepth)
    {
        Vector3 tempCube = Vector3.zero;
        foreach (Transform child in levelData)
        {
            if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
            {
                tempCube = new Vector3(child.position.x, child.position.y, newDepth);
                if (!FindTransformInvisiList(tempCube) && !FindTransformLevel(tempCube) && !FindTransformBuilding(child.position))
                {

                    Transform go = CreateInvisicube(tempCube);
                    invisiList.Add(go);
                }
            }
        }
    }

    private bool FindTransformBuilding(Vector3 cube)
    {
        foreach (Transform item in buildingData)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z < cube.z)
                {

                    return true;
                }
            }
            else if (facingDirection == FacingDirection.Back)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z > cube.z)
                {

                    return true;
                }
            }
            else if (facingDirection == FacingDirection.Right)
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x > cube.x)
                {

                    return true;
                }
            }
            else
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x < cube.x)
                {

                    return true;
                }
            }
        }
        return false;
    }

    private void ReturnToStart()
    {
        UpdateLevelData(true);
    }

    private float GetPlayerDepth()
    {
        float ClosestPoint = 0f;

        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
        {
            ClosestPoint = characterMovement.transform.position.z;

        }
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
        {
            ClosestPoint = characterMovement.transform.position.x;
        }


        return Mathf.Round(ClosestPoint);

    }

    private FacingDirection GetRotationRight()
    {
        int change = (int)(facingDirection);
        change++;

        if (change > 3)
        {
            change = 0;
        }
        return (FacingDirection)(change);
    }

    private FacingDirection GetRotationLeft()
    {
        int change = (int)(facingDirection);
        change--;
        if (change < 0)
        {
            change = 3;
        }
        return (FacingDirection)(change);

    }

}





