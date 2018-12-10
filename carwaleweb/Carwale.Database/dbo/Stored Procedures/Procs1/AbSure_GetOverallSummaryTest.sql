IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetOverallSummaryTest]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetOverallSummaryTest]
GO

	
-- =============================================
-- Author:		
-- Create date: 20th April 2015
-- Description:	To get the overall summary of the logged in user
-- EXEC [AbSure_GetOverallSummaryTEST]  13175
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetOverallSummaryTest] 
(
	@UserId          INT
)
AS
BEGIN

    DECLARE @RoleId INT
	DECLARE @IsAxaAdmin BIT=0
	
	CREATE TABLE #TempTable  
	(
		Id INT IDENTITY(1,1), 
		ChildId INT     ---  this is use  for get  all id  under given userId
	)

	CREATE  INDEX IDX_CTempTable_ChildId ON #TempTable(ChildId);

    IF (SELECT  TU.RoleId FROM TC_Users AS TU WITH(NOLOCK) WHERE TU.Id = @UserId ) = 15 --Axa Agency - 15  (Get All Surveyor Under Specific Agency)
		BEGIN
			INSERT INTO #TempTable(ChildId) EXEC TC_GetImmediateChild @UserId --To get child if user is axa super admin or agency
		END
	ELSE IF (SELECT  TU.RoleId FROM TC_Users AS TU WITH(NOLOCK) WHERE TU.Id = @UserId) = 9   -- SuperAdmin - 9 (Get all Agency Under Super Admin Axa )
		BEGIN
		SET @IsAxaAdmin=1
			INSERT INTO #TempTable(ChildId)  EXEC TC_GetLevelWiseChild @UserId,2  -- to get All surveyorsId Under Specific Axa Admin 
		END
	ELSE IF (SELECT  ISNULL(TU.RoleId,13) FROM TC_Users AS TU WITH(NOLOCK) WHERE TU.Id = @UserId) = 13
		BEGIN
			INSERT INTO #TempTable(ChildId) VALUES (@UserId) -- surveyor
		END


		IF @IsAxaAdmin=1
		    BEGIN 
		        --  Insert the data into #AxaTempTable
				
				SELECT TT.ChildId AS Surveyor, TU.UserName, TU.IsAgency, COUNT(ABC.Id) AS Allocated,
					SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS Pending,	
					
					CASE WHEN SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) = 0  THEN 0 
					ELSE CAST(ROUND(CAST(CAST(SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT ABC.Id) *100 AS DECIMAL(8,2)),0) AS INT)
					END PendingPercentage,
													 					
					SUM(CASE ISNULL(ABC.Status,0) WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 4 THEN 1 WHEN 7 THEN 1 WHEN 8 THEN 1 ELSE 0 END) AS Done,
					
					CASE WHEN COUNT(ABC.Id) =0 THEN 0 
					ELSE CAST(CAST(SUM(CASE ISNULL(ABC.Status,0) WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 4 THEN 1 WHEN 7 THEN 1 WHEN 8 THEN 1 ELSE 0 END) AS float) / COUNT(ABC.Id) *100 AS DECIMAL(8,2))
				    END DonePercentage,
				
					
					SUM(CASE ISNULL(ABC.Status,0) WHEN 3 THEN 1 ELSE 0 END) AS Cancelled,

					CASE WHEN COUNT(ABC.Id)=0 THEN 0 
					ELSE CAST(CAST(SUM(CASE ISNULL(ABC.Status,0) WHEN 3 THEN 1 ELSE 0 END) AS float) / COUNT(ABC.Id) *100 AS DECIMAL(8,2))
				    END CancelledPercentage,


					SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) >= CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS FutureAppointments,

					CASE WHEN COUNT(ABC.Id)=0 THEN 0 
					ELSE CAST(CAST(SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) >= CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS float) /COUNT(ABC.Id)  *100 AS DECIMAL(8,2))
				    END FutureAppointmentsPercentage,

					SUM(CASE WHEN ISNULL(ABC.IsInspectionRescheduled,0) = 1  THEN 1 ELSE 0 END) AS Rescheduled,

					CASE WHEN COUNT(ABC.Id)=0 THEN 0 
					ELSE CAST(CAST(SUM(CASE WHEN ISNULL(ABC.IsInspectionRescheduled,0) = 1  THEN 1 ELSE 0 END) AS float) / COUNT(ABC.Id) *100 AS DECIMAL(8,2))
				    END RescheduledPercentage,


					CASE WHEN SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) THEN 1 ELSE 0 END) = 0 THEN 0 
					ELSE CAST(ROUND(CAST(CAST((SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) AND ABC.SurveyDate > ABC.AppointmentDate THEN 1 ELSE 0 END)) AS float) 
					    / (SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) THEN 1 ELSE 0 END))*100 AS DECIMAL(8,2)),0) AS INT)
					END delay,


					(SELECT Id FROM TC_Users WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId = (SELECT BranchId FROM TC_Users WHERE Id=@UserId) /*local= 14168  staging =11165*/) AS ParentID,
					(SELECT UserName FROM TC_Users WHERE HierId =  TU.HierId.GetAncestor(1) AND BranchId =(SELECT BranchId FROM TC_Users WHERE Id=@UserId)  /* 14168 11165*/ ) AS Parent
				   	 INTO #AxaTempTable
					
					FROM #TempTable AS TT
					     INNER JOIN  TC_Users AS TU WITH (NOLOCK) ON TU.Id = TT.ChildId
						 LEFT JOIN AbSure_CarSurveyorMapping ABCS WITH (NOLOCK) ON TT.ChildId = ABCS.TC_UserId
						 LEFT JOIN AbSure_CarDetails ABC WITH (NOLOCK) ON ABC.Id = ABCS.AbSure_CarDetailsId
					WHERE ISNULL(TU.IsAgency,0) = 0 
					GROUP BY TT.ChildId, TU.UserName, TU.IsAgency,TU.HierId.GetAncestor(1)
					ORDER BY  TU.UserName			
							 
			      SELECT COUNT(Surveyor) AS TotalSurveyor ,'1' AS IsAgency ,TD.ParentID AS Id , TD.Parent AS UserName,SUM(TD.Allocated) Allocated,
					  SUM(TD.Pending) Pending, 
					  CASE WHEN SUM(TD.Allocated) = 0 THEN 0 ELSE CAST(ROUND(SUM(TD.Pending)*100.0/SUM(TD.Allocated),0) AS INT)  END  PendingPercentage,
					  SUM(TD.Done) Done,
					  CASE WHEN SUM(TD.Allocated)  = 0 THEN 0 ELSE CAST(ROUND(SUM(TD.Done)*100.0/SUM(TD.Allocated),0) AS INT)  END DonePercentage,
					  SUM(TD.FutureAppointments) FutureAppointments ,	  
					  CASE WHEN  SUM(TD.FutureAppointments) = 0 THEN 0 ELSE CAST(ROUND(SUM(TD.FutureAppointments)*100.0/SUM(TD.Allocated),0) AS INT) END FutureAppointmentsPercentage,
					  SUM(TD.Cancelled) Cancelled ,
					  CASE WHEN SUM(TD.Cancelled) = 0 THEN 0 ELSE CAST(ROUND(SUM(TD.Cancelled)*100.0/SUM(TD.Allocated),0) AS INT) END CancelledPercentage,
					  SUM(TD.Rescheduled)  Rescheduled,
					  CASE WHEN SUM(TD.Rescheduled) = 0 THEN 0 ELSE CAST(ROUND(SUM(TD.Rescheduled)*100.0/SUM(TD.Allocated),0) AS INT) END RescheduledPercentage,
					  CASE WHEN COUNT(Surveyor) = 0 THEN 0 ELSE ROUND(CAST(SUM(TD.delay)/COUNT(Surveyor) AS float),0) END delay
  			 						   
				 FROM #AxaTempTable AS TD				 
 				 WHERE TD.ParentID IS NOT NULL 
				 GROUP BY TD.ParentID ,TD.IsAgency,TD.Parent 
				 ORDER BY TD.Parent 	 			 	
				
				DROP TABLE #AxaTempTable	

		  END
		ELSE
			BEGIN 
				    SELECT TT.ChildId AS Id, TU.UserName,ISNULL(TU.IsAgency,0) AS IsAgency ,
					    COUNT(ABC.Id) AS Allocated,
						SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS Pending,	
					
						CASE WHEN SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) = 0  THEN 0 
						ELSE CAST(ROUND(CAST(CAST(SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) < CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT ABC.Id) *100 AS DECIMAL(8,2)),0) AS INT)
						END PendingPercentage,
													 					
						SUM(CASE ISNULL(ABC.Status,0) WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 4 THEN 1 WHEN 7 THEN 1 WHEN 8 THEN 1 ELSE 0 END) AS Done,
					
						CASE WHEN COUNT(ABC.Id) =0 THEN 0 
						ELSE CAST(ROUND(SUM(CASE ISNULL(ABC.Status,0) WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 4 THEN 1 WHEN 7 THEN 1 WHEN 8 THEN 1 ELSE 0 END)*100.0 / COUNT(ABC.Id),0) AS INT)
						END DonePercentage,
				
					
						SUM(CASE ISNULL(ABC.Status,0) WHEN 3 THEN 1 ELSE 0 END) AS Cancelled,

						CASE WHEN COUNT(ABC.Id)=0 THEN 0 
						ELSE CAST(ROUND(SUM(CASE ISNULL(ABC.Status,0) WHEN 3 THEN 1 ELSE 0 END)*100.0  / COUNT(ABC.Id),0) AS INT) 
						END CancelledPercentage,


						SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) >= CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END) AS FutureAppointments,
						
						CASE WHEN COUNT(ABC.Id)=0 THEN 0 
						ELSE CAST(ROUND(SUM(CASE WHEN ISNULL(ABC.Status,0) = 0 AND CONVERT(DATE, ABC.AppointmentDate) >= CONVERT(DATE, GETDATE()) THEN 1 ELSE 0 END)*100.0 /COUNT(ABC.Id),0) AS INT)
						END FutureAppointmentsPercentage,

						SUM(CASE WHEN ISNULL(ABC.IsInspectionRescheduled,0) = 1  THEN 1 ELSE 0 END) AS Rescheduled,				

						CASE WHEN COUNT(ABC.Id)=0 THEN 0 
						ELSE CAST(ROUND(SUM(CASE WHEN ISNULL(ABC.IsInspectionRescheduled,0) = 1  THEN 1 ELSE 0 END)*100.0 / COUNT(ABC.Id),0) AS INT)
						END RescheduledPercentage,
						
						CASE WHEN SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) THEN 1 ELSE 0 END) = 0 THEN 0 
						ELSE CAST(ROUND(CAST(CAST((SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) AND ABC.SurveyDate > ABC.AppointmentDate THEN 1 ELSE 0 END)) AS float) 
							/ (SUM(CASE WHEN ABC.Status IS NULL AND ABC.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ABC.Status IN (1,2) THEN 1 ELSE 0 END))*100 AS DECIMAL(8,2)),0) AS INT)
						END delay					
					FROM #TempTable AS TT
					     INNER JOIN  TC_Users AS TU WITH (NOLOCK) ON TU.Id = TT.ChildId
						 LEFT JOIN AbSure_CarSurveyorMapping ABCS WITH (NOLOCK) ON TT.ChildId = ABCS.TC_UserId
						 LEFT JOIN AbSure_CarDetails ABC WITH (NOLOCK) ON ABC.Id = ABCS.AbSure_CarDetailsId
					WHERE ISNULL(TU.IsAgency,0) = 0 
					GROUP BY TT.ChildId, TU.UserName, TU.IsAgency
					ORDER BY  TU.UserName				
	    END
	   DROP TABLE #TempTable
END
