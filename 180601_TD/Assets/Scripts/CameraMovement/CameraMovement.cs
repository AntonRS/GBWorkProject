using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CameraMovement
{
    /// <summary>
    /// Вешается на камеру, обеспечивает движение камеры
    /// </summary>
    public class CameraMovement : MonoBehaviour
    {

        [Tooltip("Ширина расположенных по краям экрана секторов, при попадании в которых курсора мыши камера двигается в соответствующем направлении")]
        [SerializeField]
        [Range(0.01f, 0.3f)]
        private float _mouseMoveSideTriggerWidth;

        [SerializeField]
        private float _cameraMoveSpeed;

        private float _rightBorder;
        private float _leftBorder;
        private float _topBorder;
        private float _bottomBorder;

        private Vector3 _mousePosition;
        private float _horizontalAxisValue;
        private float _verticalAxisValue;

        // Use this for initialization
        void Awake()
        {

            _leftBorder = Screen.width * _mouseMoveSideTriggerWidth;
            _bottomBorder = Screen.height * _mouseMoveSideTriggerWidth;

            _rightBorder = Screen.width - _leftBorder;
            _topBorder = Screen.height - _bottomBorder;

        }



        // Update is called once per frame

        //не забыть выключить, когда мы в главном меню, а не в основной игре
        //проще всего это сделать, задизейблив скрипт при выходе в меню
        void Update()
        {


            _mousePosition = Input.mousePosition;

            _horizontalAxisValue = Input.GetAxisRaw("Horizontal");
            _verticalAxisValue = Input.GetAxisRaw("Vertical");


            //по сути, мы дальше проверяем те же условия ещё раз. Мне кажется, первая проверка лишняя. Закомментил - но если надо, верну.
            //if (
            //        _mousePosition.x > _leftBorder
            //        && _mousePosition.x < _rightBorder
            //        && _mousePosition.y > _bottomBorder
            //        && _mousePosition.y < _topBorder

            //        && _horizontalAxisValue == 0
            //        && _verticalAxisValue == 0
            //    )
            //    return;

            
            if (_mousePosition.x <= _leftBorder || _horizontalAxisValue < 0)
                MoveCamera(new Vector3(-_cameraMoveSpeed, 0, 0));

            if (_mousePosition.x >= _rightBorder || _horizontalAxisValue > 0)
                MoveCamera(new Vector3(_cameraMoveSpeed, 0, 0));

            if (_mousePosition.y <= _bottomBorder || _verticalAxisValue < 0)
                MoveCamera(new Vector3(0, 0, -_cameraMoveSpeed));

            if (_mousePosition.y >= _topBorder || _verticalAxisValue > 0)
                MoveCamera(new Vector3(0, 0, _cameraMoveSpeed));

        }

        /// <summary>
        /// Сдвигает камеру по подаваемому на вход вектору
        /// </summary>
        /// <param name="MoveIncrement"></param>
        private void MoveCamera(Vector3 moveIncrement)
        {
            transform.position += moveIncrement * Time.deltaTime;
        }
    }
}