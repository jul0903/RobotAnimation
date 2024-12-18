GRADIENT DESCENT METHOD

Link al repo: https://github.com/jul0903/RobotAnimation

Júlia Martos Moraleda
Nerea Martinez León

IK Implementation
Para implementar el movimiento de los joints, decidimos probar con una versión pequeña del brazo (con solo 4 joints) a partir de la clase y script de Gradient Descent con Quaternions:
Para ello, hemos creado un script “GradientArm”, que irtera una lista de joints (desde el 0 hasta el endfactor) y calcula y aplica la rotación del siguiente joint en función del gradient y coste. Se basa en la idea de minimizar el coste (distancia entre el endfactor y target).

“Gradient Arm”
Guarda los joints y distancias (links) del brazo. Si no ha llegado al target (costfunc > tolerance):
 - Calcula la gradiente de la función de costo con GetGradient() usando diferencias finitas, es decir, calculando cuántos steps hay que ajustar a cada ángulo de rotación.
 - Actualiza theta usando gradient descendient con la función lossCostFunction. 
 - Calcula las posiciones nuevas con endFactorFunction() y actualiza las posiciones de cada Joint. Utiliza quaternions y las rotaciones se van acumulando. Al final, ajusta la orientación del endfactor para que mire hacia el target.
Lo hemos querido hacer así ya que al ser modular, podemos poner tantos joints como queramos.


“Gradient Claw”
Para las garras del brazo hemos hecho un script a parte, referenciando al script del brazo para poder comprobar la tolerancia.
Guarda las variables de rotación iniciales y a la hora de cerrar la garra (Close claw) interpola las rotaciones de los joints hacia el target, que va indicado por un ángulo que le hemos puesto nosotras. En este caso, todas las garras tienen que rotar en el mismo eje y un mismo ángulo, es por eso que hemos puesto 30º en el eje x.
Hace lo mismo para abrir la garra pero con las variables al revés: es por eso que guardamos las rotaciones iniciales.

“Player Movement”
Movimiento simple de jugador con WASD y SPACE para saltar.
