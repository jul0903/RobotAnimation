Nerea Matrtínez y Júlia Martos

Link a GitHub: https://github.com/jul0903/RobotAnimation.git

Escena Ex 1 (Script: RobotController):
Update() --> Iniciamos el movimiento con la tecla "M".

MoveToTargetPosition (float[] targetAngles) --> Colocamos los joints en los ángulos iniciales para después moverlos hacia el target según los ángulos deseados.

SetJointAngles(float[] angles) --> Se aplican las rotaciones para llegar a los ángulos deseados.


Escena Inverse Kinematic (Script: FordwardKinematic2 y Joint):
- Joint
Awake() --> Seteamos el offset y el axis (el eje en el que se van a aplicar las rotaciones) de cada joint. Se hace mdiante el inspector en cada joint.

- ForwardKinematics2
GetJoints() --> Getter de joints.

ForwardKin(float[] angles, Joint[] joints) --> Se calculan las rotaciones en los axis que estan definidos en cada joint.

MoveToTarget() --> Mediante CCD Creamos un vector hacia la dirección del target y a partir de la dirección a la que está mirando el brazo calcula el ángulo entre uno y otro y lo suma en cada joint para que éstos vayan hacia el target.


Webs utilizadas:
https://www.alanzucconi.com/2017/04/06/implementing-forward-kinematics/