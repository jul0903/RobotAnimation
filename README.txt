IK con GRADIENT DESCENT METHOD

Link al repo: https://github.com/jul0903/RobotAnimation

Júlia Martos Moraleda
Nerea Martinez León

El proyecto de Unity funciona con varios scripts en función del objeto.
Path: Assets > Scenes > AA3

Scripts
Tenemos 3:
FABRIK → Está en el armature del brazo. Controla el movimiento de éste con 35 joints mediante el método FABRIK. También hemos añadido que la garra apunte hacia el target.
El brazo siempre persigue al dron, y una vez la distancia es menor a la tolerancia, se espera 2 segundos y le devuelve el dron al astronauta. Un problema al que nos hemos enfrentado, es que a pesar de poner diferentes funciones de Forward(), el movimiento de retorno al astronauta lo teletransporta.
DroneMovement → Controla el movimiento del Dron. De default está en manual y se controla con WASD y QE para mover y subir y bajar, pero si pulsas la tecla 1, se cambia a un conjunto de posiciones predefinidas. Una vez el brazo coje el dron, este desactiva su movimiento para evitar errores, y en el script de FABRIK pasa a ser hijo del brazo para poder moverse con él.
CCDFromBottom → Se aplica en cada brazo. Ya que nuestro astronauta tiene los brazos cortos, le hemos añadido varios joints para que se vea mejor. 
Los brazos están constantemente buscando el target ( el dron).
Hemos tratado de aplicar constraints en base al ángulo y eje, pero nos ha dado muchos problemas así que hemos priorizado pulir otros aspectos de la práctica.  
