using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MatlabIK : MonoBehaviour
{
    void Update()
    {
        
    }

    Vector3 lastPos
    , lastRot;

    double[] UR3InvK(double[] inputs)
    {
        double t_lapse = inputs[19] - inputs[20];

        if (inputs[19] > 0.5)
        {
            if (lastPos == null)
                lastPos = Vector3.zero;
            if (lastRot == null)
                lastRot = Vector3.zero;

            int steps = 5;
            double[,] joint = new double[6, steps + 1];
            double[,] jointWS = new double[6, 2];
            double[,] origin = new double[3, 6];
            double[,] axis = new double[3, 6];
            double[,] pEnd = new double[3, steps];
            double[,] jaco = new double[6, 6];
            double[,] pTarget = new double[3, steps + 1];
            double[,] FATarget = new double[3, steps + 1];
            double[,] oDelta = new double[3, steps];
            double[,] wJointLtd = new double[6, 6];
            double[,] jointLast = new double[6, 1];
            double[,] rTeleDelta = new double[3, 3];
            double[,] finalPos = new double[3, 1];
            double[,] finalRxRyRz = new double[3, 1];
            double[,] pTeleDelta = new double[3, 1];
            double[,] RxRyRzTeleDelta = new double[3, 1];
            double[,] RJ5 = new double[3, 3];

            //Initial Joint Position
            joint[0, 0] = inputs[7] * Mathf.Deg2Rad;
            joint[1, 0] = inputs[8] * Mathf.Deg2Rad;
            joint[2, 0] = inputs[9] * Mathf.Deg2Rad;
            joint[3, 0] = inputs[10] * Mathf.Deg2Rad;
            joint[4, 0] = inputs[11] * Mathf.Deg2Rad;
            joint[5, 0] = inputs[12] * Mathf.Deg2Rad;

            //Physical Dimensions
            double[,] jointLtd = new double[,]
            {
                { -360, 360 },
                { -360, 360 },
                { -360, 360 },
                { -360, 360 },
                { -360, 360 },
                { -9000, 9000 },
            };

            double jointLtdExRate = 50;
            double jointSB = 20;
            for (int i = 0; i < 6; i++)
            {
                jointWS[i, 0] = (jointLtd[i, 1] + jointSB) * Mathf.Deg2Rad;
                jointWS[i, 1] = (jointLtd[i, 1] + jointSB) * Mathf.Deg2Rad;
            }

            double4x4 T = new double4x4(new double4(1, 0, 0, 0), new double4(0, 1, 0, 0), new double4(0, 0, 1, 0), new double4(0, 0, 0, 1));
            double4 pTool = new double4 (0, 0, 0.03, 1);

            double[,] wEnd = new double[,]
            {
                { 1, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0.01, 0, 0 },
                { 0, 0, 0, 0, 0.01, 0 },
                { 0, 0, 0, 0, 0, 0.01 },
            };

            double[,] wJoint = new double[,]
            {
                { 1, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 1 },
            };

            double DF2 = 0.05; //Damping Factor
            
            for (int k = 0; k < steps; k++)
            {
                double[,] dh = new double[,]
                {
                    { 0,            0,      0.152,  joint[0, k]},
                    { Math.PI / 2,  0,      0,      joint[1, k]},
                    { 0,            0.244,  0,      joint[2, k]},
                    { 0,            0.213,  0.11,   joint[3, k]},
                    { Math.PI / 2,  0,      0.0834, joint[4, k]},
                    { -Math.PI / 2, 0,      0.0826, joint[5, k]}
                };

                for (int m = 0; m < 6; m++)
                {
                    T = T * new double4x4(
                        new double4(cos(dh[m, 3]) - sin(dh[m, 3]),    0,                                                                        dh[m, 1],               0),
                        new double4(sin(dh[m, 3]) * cos(dh[m, 0]),    cos(dh[m, 3]) * cos(dh[m, 0]) - sin(dh[m, 0]) - sin(dh[m, 0]) * dh[m, 2], 0,                      0),
                        new double4(sin(dh[m, 3]) * sin(dh[m, 0]),    cos(dh[m, 3]) * sin(dh[m, 0]),                                            cos(dh[m, 0]),          cos(dh[m, 0]) * dh[m, 2]),
                        new double4(0,                                0,                                                                        0,                      1)
                        );
                    axis[0, m] = T[0][2]; 
                    axis[1, m] = T[1][2]; 
                    axis[2, m] = T[2][2];

                    origin[0, m] = T[0][3];
                    origin[1, m] = T[1][3];
                    origin[2, m] = T[2][3];

                    //if (m == 5)
                    //    RJ5 = new double[,]
                    //    {
                    //        { T[0][0], T[0][1], T[0][2] },
                    //        { T[1][0], T[1][1], T[1][2] },
                    //        { T[2][0], T[2][1], T[2][2] }
                    //    };
                }

                double4 pEndTemp = matrixVectorProduct(T, pTool);
                pEnd[0, k] = pEndTemp[0]; 
                pEnd[1, k] = pEndTemp[1]; 
                pEnd[2, k] = pEndTemp[2];

                jaco = assignVector(jaco, 0, 0, Vector3.Cross(matrixToVector(axis, 0, 0), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 0)));
                jaco = assignVector(jaco, 0, 1, Vector3.Cross(matrixToVector(axis, 0, 1), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 1)));
                jaco = assignVector(jaco, 0, 2, Vector3.Cross(matrixToVector(axis, 0, 2), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 2)));
                jaco = assignVector(jaco, 0, 3, Vector3.Cross(matrixToVector(axis, 0, 3), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 3)));
                jaco = assignVector(jaco, 0, 4, Vector3.Cross(matrixToVector(axis, 0, 4), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 4)));
                jaco = assignVector(jaco, 0, 5, Vector3.Cross(matrixToVector(axis, 0, 5), matrixToVector(pEnd, 0, k) - matrixToVector(origin, 0, 5)));

                jaco = assignVector(jaco, 3, 0, matrixToVector(axis, 0, 0));
                jaco = assignVector(jaco, 3, 1, matrixToVector(axis, 0, 1));
                jaco = assignVector(jaco, 3, 2, matrixToVector(axis, 0, 2));
                jaco = assignVector(jaco, 3, 3, matrixToVector(axis, 0, 3));
                jaco = assignVector(jaco, 3, 4, matrixToVector(axis, 0, 4));
                jaco = assignVector(jaco, 3, 5, matrixToVector(axis, 0, 5));

                if (k == 1)
                {
                    Vector3 pInitial = matrixToVector(pEnd, 0, 0);
                    double3x3 rInitial = shrinkMatrix(T, 0, 0);

                    pTarget[0, 0] = pInitial[0];
                    pTarget[1, 0] = pInitial[1];
                    pTarget[2, 0] = pInitial[2];
                    
                }
            }
        }
    }

    double3x3 shrinkMatrix(double4x4 matrix, int startCol, int startRow)
    {
        return new double3x3(
                matrix[startRow][startCol], matrix[startRow][startCol + 1], matrix[startRow][startCol + 2],
                matrix[startRow + 1][startCol], matrix[startRow + 1][startCol + 1], matrix[startRow + 1][startCol + 2],
                matrix[startRow + 2][startCol], matrix[startRow + 2][startCol + 1], matrix[startRow + 2][startCol + 2]
            );
    }
    Vector3 matrixToVector(double[,] matrix, int startCol, int row) { return new Vector3((float)matrix[row, startCol], (float)matrix[row, startCol + 1], (float)matrix[row, startCol + 2]); }
    double[,] assignVector(double[,] matrix, int startCol, int row, Vector3 vector)
    {
        matrix[row, startCol] = vector[0];
        matrix[row, startCol + 1] = vector[1];
        matrix[row, startCol + 2] = vector[2];

        return matrix;
    }
    double4 matrixVectorProduct(double4x4 matrix, double4 vector)
    {
        return new double4(
            matrix[0][0] * vector[0] + matrix[0][1] * vector[1] + matrix[0][2] * vector[2] + matrix[0][3] * vector[3],
            matrix[1][0] * vector[0] + matrix[1][1] * vector[1] + matrix[1][2] * vector[2] + matrix[1][3] * vector[3],
            matrix[2][0] * vector[0] + matrix[2][1] * vector[1] + matrix[2][2] * vector[2] + matrix[2][3] * vector[3],
            matrix[3][0] * vector[0] + matrix[3][1] * vector[1] + matrix[3][2] * vector[2] + matrix[3][3] * vector[3]
        );
    }
    double sin(double i) { return Math.Sin(i); }
    double cos(double i) { return Math.Cos(i); }

}

//function AxAyAZ= R2FixAngle(R) 

//MINA = 0.0001; % minimun we consider to be 0 degree

//AY= atan2(-R(3,1), sqrt(R(1, 1) ^ 2 + R(2, 1) ^ 2));

//CAY = cos(AY);

//if abs(CAY) > MINA
//    AZ = atan2(R(2, 1) / CAY, R(1, 1) / CAY);
//AX = atan2(R(3, 2) / CAY, R(3, 3) / CAY);
//elseif AY>0 % AY=90 degree
//   AY=pi/2;
//AZ = 0; % this is a convention to choose AZ to be zero, see page 47
//   AX=atan2(R(1,2), R(2, 2));
//else  % AY = -90 degree
//     AY = -pi / 2;
//AZ = 0; % this is a convention to choose AZ to be zero, see page 47
//   AX=-atan2(R(1,2), R(2, 2));
//end
//AxAyAZ = [AX AY AZ]';


//if k == 1 % cal the initial posotition and orientation from the initial joint reading
//%target position, should use the sensor reading to calculate the X, Y, Z and
//%plan the end effector motion accordingly to avoid rapid movement at start

//PInitial    = PEnd(1:3,1);
//RInitial = T(1:3, 1:3);


//% Initialize the tip motion Target 
//PTarget(1, 1) = PInitial(1);
//PTarget(2, 1) = PInitial(2);
//PTarget(3, 1) = PInitial(3);

//InitialRxRyRZ = R2FixAngle(RInitial);

//FATarget(1:3, 1) = InitialRxRyRZ; %using Fixed-Angle representation page 47, intro to robotics

//% set the final target position and RxRyRz
//    if Inputs(13) >= 1 % 1: tele - operation on end-effector, using Mode switching Initial pos + delat pos as the final target 
//        magPosCommand = norm(Inputs(14:16));
//magRotCommand = norm(Inputs(17:19));

//if magPosCommand < 0.2 % no motion needed
//         PTeleDelta = zeros(3, 1);
//     else
//    PTeleDelta = RInitial * Inputs(14:16) * t_lapse * 0.1;
//end

//     if magRotCommand < 0.2 % no rotation needed
//        RTeleDelta = eye(3, 3);
//     else
//    RxRyRzTeleDelta = Inputs(17:19) * t_lapse * 0.5;
//RTeleDelta = FixAngle2R(RxRyRzTeleDelta(1:3));
//end

//   finalPos = PositionLast + PTeleDelta;
//finalRxRyRz = R2FixAngle(FixAngle2R(RotationLast * Degree2Radian) * RTeleDelta) * 180 / pi;
//        % finalRxRyRz = Inputs(4:6);


//    else % = 0 path following
//        finalPos = Inputs (1:3);
//finalRxRyRz = Inputs(4:6);
//end
//end    


//%linear planning 
//PTarget(1, k+1) = PTarget(1, 1) + (finalPos(1) - PTarget(1, 1)) / steps * k;
//PTarget(2, k + 1) = PTarget(2, 1) + (finalPos(2) - PTarget(2, 1)) / steps * k;
//PTarget(3, k + 1) = PTarget(3, 1) + (finalPos(3) - PTarget(3, 1)) / steps * k;
//% PTarget(3, k + 1) = PTarget(3, 1) - RD - RD * cos(pi + 2 * pi * k / steps);


//% planning the orientation motion

//FATarget(1, k+1)= finalRxRyRz(1) * Degree2Radian;
//FATarget(2, k + 1) = finalRxRyRz(2) * Degree2Radian;
//FATarget(3, k + 1) = finalRxRyRz(3) * Degree2Radian;

//RTarget = FixAngle2R(FATarget(1:3, k + 1));

//% tip motion delta
//PDelta= PTarget(1:3,k + 1)-PEnd(1:3, k);
//% Rotation error
// Rerror = RTarget * (T(1:3,1:3)');
//ODelta(1:3, k) = 0.5 *[Rerror(3, 2) - Rerror(2, 3); Rerror(1, 3) - Rerror(3, 1); Rerror(2, 1) - Rerror(1, 2)];

//% Least square pseudo inverse of J
//JacoP=Jaco(1:6,1:6);
//% Jw = transpose(JacoP) * inv(JacoP * transpose(JacoP) + DF2 * eye(6));


//% Joint limit avoidance
//for i=1:6
//    if Joint(i, k) > JointWS(i, 2)
//        JointOver = Joint(i, k) - JointWS(i, 2);
//elseif Joint(i, k) < JointWS(i, 1)
//        JointOver = JointWS(i, 1) - Joint(i, k);
//    else
//    JointOver = 0;
//end
//WJointLtd(i, i)= WJoint(i, i) * exp(JointLtdExRate * JointOver); % Augment the WJoint weighting matrix with the Joint Limit Avoidance, closer to the limit, the Weighting unit will increase exponentially, and leading to smaller velocity till stop, therefore avoiding exceeding the limit
//end

//Jw=inv(transpose(JacoP)*WEnd * JacoP + DF2 * (WJointLtd))*transpose(JacoP) * WEnd;

//% conJ = cond(JacoP)
//% Jw = inv(Jaco);
//% inverse kinematic of delta joint motion 

//JDelta= Jw*[PDelta;ODelta(1:3, k)] ;

//% orientation only
// % Jw = W1 * transpose(Jaco(4:6, 1:6)) * inv(Jaco(4:6, 1:6) * W1 * transpose(Jaco(4:6, 1:6)));
//% JDelta = Jw *[ODelta];

//% with error on joint trackin
//%Joint(1:6,k + 2)= Joint(1:6, k + 1) + JDelta + dot(JDelta, [0.5 * (rand - 0.5); 0.5 * (rand - 0.5); 0.5 * (rand - 0.5); 0.5 * (rand - 0.5); 0.2 * (rand - 0.5); 0.2 * (rand - 0.5)]);
//% here is what the robot arm controller should do: drive the arm to the
//%position we need:
//Joint(1:6, k + 1) = Joint(1:6, k) + JDelta;
//end


//JointLast(1:6) = Joint(1:6, k + 1) / Degree2Radian;
//PositionLast = finalPos; % storrage of last pos target command 
//RotationLast = finalRxRyRz;

//else
//    % initial position
//     JointLast = [0, 45, -75, 120, 90, 0]';
//    PositionLast =[0.4,-0.110,0.08] ';
//    RotationLast = [180, 0, -90]';
//end


//%Fixed Angle representation page 45, intro to robotics
//%Definition: Describing the orientation of frame B is as follows:
//% start  with the frame coincident with a known reference frame A. First
//%rotate B about axis XA by an angle AngleX, then rotate about YA by an
//%angle AngleY, and then rotate about ZA by an angle AngleZ.

//function [R]= FixAngle2R(AngleXYZ)

//cX = cos(AngleXYZ(1));
//sX = sin(AngleXYZ(1));
//cY = cos(AngleXYZ(2));
//sY = sin(AngleXYZ(2));
//cZ = cos(AngleXYZ(3));
//sZ = sin(AngleXYZ(3));


//R =[cZ * cY cZ * sY * sX - sZ * cX cZ * sY * cX + sZ * sX;
//sZ* cY sZ*sY*sX+cZ*cX sZ*sY*cX-cZ*sX;
//-sY  cY* sX   cY*cX];





