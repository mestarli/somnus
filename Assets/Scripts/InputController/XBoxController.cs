/// <summary>
/// XBox Una clase de información de asignación de claves del controlador.
/// La propiedad del eje se usa en el método Input.GetAxis (string name);
/// La propiedad clave se usa en el método Input.GetKey (nombre de cadena).
/// Todas las propiedades no se aplican al método Input.GetButton (nombre de cadena), si desea utilizar este método para botones,
/// Se debe pasar el valor de la propiedad "Nombre" de este botón en la página "Administrador de entrada" de Unity, no el valor de la propiedad "Botón positivo".
/// </summary>
public class XBoxController
{
    #region  Eje del palo (palo)

    /// <summary>
    /// Left Stick Horizontal.
    /// Axis: X Axis.
    /// </summary>
    public string LSH { get { return "LSH"; } }

    /// <summary>
    /// Left Stick Vertical.
    /// Axis: Y Axis.
    /// </summary>
    public string LSV { get { return "LSV"; } }

    /// <summary>
    /// Right Stick Horizontal.
    /// Axis: 4th Axis.
    /// </summary>
    public string RSH { get { return "RSH"; } }

    /// <summary>
    /// Right Stick Vertical.
    /// Axis: 5th Axis.
    /// </summary>
    public string RSV { get { return "RSV"; } }

    #endregion

    #region  Pad direccional (Pad direccional, D-Pad)

    /// <summary>
    /// D-Pad Horizontal.
    /// Axis: 6th Axis.
    /// </summary>
    public string DPadH { get { return "DPadH"; } }

    /// <summary>
    /// D-Pad Vertical.
    /// Axis: 7th Axis.
    /// </summary>
    public string DPadV { get { return "DPadV"; } }

    #endregion

    #region  Eje de gatillo (gatillo)

    /// <summary>
    /// Left Trigger.
    /// Axis: 9th Axis.
    /// </summary>
    public string LT { get { return "LT"; } }

    /// <summary>
    /// Right Trigger.
    /// Axis: 10th Axis.
    /// </summary>
    public string RT { get { return "RT"; } }

    /// <summary>
    /// Shared Trigger.
    /// Axis: 3rd Axis.
    /// </summary>
    public string Trigger { get { return "Trigger"; } }

    #endregion

    #region  Tecla ABXY

    /// <summary>
    /// A.
    /// Positive Button: joystick button 0.
    /// </summary>
    public string A { get { return "joystick button 0"; } }
    /// <summary>
    /// B.
    /// Positive Button: joystick button 1.
    /// </summary>
    public string B { get { return "joystick button 1"; } }
    /// <summary>
    /// X.
    /// Positive Button: joystick button 2.
    /// </summary>
    public string X { get { return "joystick button 2"; } }
    /// <summary>
    /// Y.
    /// Positive Button: joystick button 3.
    /// </summary>
    public string Y { get { return "joystick button 3"; } }

    #endregion

    #region  Tecla de búfer (parachoques)

    /// <summary>
    /// Left Bumper.
    /// Positive Button: joystick button 4.
    /// </summary>
    public string LB { get { return "joystick button 4"; } }

    /// <summary>
    /// Right Bumper.
    /// Positive Button: joystick button 5.
    /// </summary>
    public string RB { get { return "joystick button 5"; } }

    #endregion

    #region  Ver (Atrás), Menú (Inicio), Teclas XBox

    /// <summary>
    /// View.
    /// Positive Button: joystick button 6.
    /// </summary>
    public string View { get { return "joystick button 6"; } }

    /// <summary>
    /// Munu.
    /// Positive Button: joystick button 7.
    /// </summary>
    public string Menu { get { return "joystick button 7"; } }

    // cadena pública XBox {get {return string.None;}} // sin correspondencia

    #endregion

    #region  Llave de joystick

    /// <summary>
    /// Left Stick Button.
    /// Positive Button: joystick button 8.
    /// </summary>
    public string LS { get { return "joystick button 8"; } } // JoystickButton8

    /// <summary>
    /// Right Stick Button.
    /// Positive Button: joystick button 9.
    /// </summary>
    public string RS { get { return "joystick button 9"; } } // JoystickButton9

    #endregion
}
