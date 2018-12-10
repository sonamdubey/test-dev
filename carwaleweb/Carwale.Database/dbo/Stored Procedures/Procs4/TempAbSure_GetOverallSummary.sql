IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TempAbSure_GetOverallSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TempAbSure_GetOverallSummary]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th April 2015
-- Description:	To get the overall summary of the logged in user
-- Modified By : Vinay Kumar Prajapati  20th April 2015 
-- EXEC [AbSure_GetOverallSummary] 20074,NULL,NULL
-- =============================================
CREATE PROCEDURE [dbo].[TempAbSure_GetOverallSummary] 
(
	@UserId          INT,
	@FromIndex       INT = NULL, 
    @ToIndex         INT = NULL
)
AS

BEGIN
	DECLARE @RoleId INT
	DECLARE @IsAxaAdmin BIT=0
	
	CREATE TABLE TempAxaTable  
	(
		Id INT IDENTITY(1,1), 
		ChildId INT     ---  this is use  for get  all id  under given userId
	)

	CREATE TABLE  AxaTempTable 
	(
	    Id INT IDENTITY(1,1), 
		UserId INT,
		IsAgency BIT,
		UserName VARCHAR(100),
		lvl INT,
		HierId HierarchyId,
		HierarchyRank INT	
	)


	--SELECT @RoleId = RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId=@UserId
	
	--IF @RoleId = 15 --Axa Agency - 15  (Get All Surveyor Under Specific Agency)
	--	BEGIN
	--		INSERT INTO TempAxaTable(ChildId) EXEC TC_GetImmediateChild @UserId --To get child if user is axa super admin or agency
	--	END
	--ELSE IF @RoleId = 9   -- SuperAdmin - 9 (Get all Agency Under Super Admin Axa )
	--	BEGIN
	--	SET @IsAxaAdmin=1
	--		INSERT INTO @AxaTempTable(UserId,IsAgency,UserName,lvl,HierId,HierarchyRank) EXEC TC_GetAllChildAbsure @UserId
	--	END
	--ELSE IF @RoleId IN (13)
	--	BEGIN
	--		INSERT INTO TempAxaTable(ChildId) VALUES (@UserId) -- surveyor
	--	END
	IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @UserId AND RoleId = 15) = 15 --Axa Agency - 15  (Get All Surveyor Under Specific Agency)
		BEGIN
			INSERT INTO TempAxaTable(ChildId) EXEC TC_GetImmediateChild @UserId --To get child if user is axa super admin or agency
		END
	ELSE IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @UserId AND RoleId = 9) = 9   -- SuperAdmin - 9 (Get all Agency Under Super Admin Axa )
		BEGIN
		SET @IsAxaAdmin=1
			INSERT INTO AxaTempTable(UserId,IsAgency,UserName,lvl,HierId,HierarchyRank) EXEC TC_GetAllChildAbsure @UserId
		END
	ELSE IF (SELECT DISTINCT RoleId FROM TC_UsersRole WITH(NOLOCK) WHERE UserId = @UserId AND RoleId IN (13)) = 13
		BEGIN
			INSERT INTO TempAxaTable(ChildId) VALUES (@UserId) -- surveyor
		END
		IF @IsAxaAdmin=1
		    BEGIN 

		        --  Insert the data into #AxaTableData

				SELECT ISNULL(TU.IsAgency,0) IsAgency ,TU.Id,TT.HierarchyRank, --TU.UserName,
				COUNT(DISTINCT CSM.AbSure_CarDetailsId) Allocated,
				SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END) Pending,
				CASE 
					WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
						THEN 0 
					ELSE
						CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2))
				END PendingPercentage,
				SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) Done,
				CASE 
					WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
						THEN 0 
					ELSE
						CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2))
				END DonePercentage,
				SUM(CASE WHEN ACD.AbsureStatus IS NULL AND acd.AppointmentDate >= GETDATE() THEN 1 ELSE 0 END) FutureAppointments,
				CASE 
					WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
						THEN 0 
					ELSE
						CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND acd.AppointmentDate >= GETDATE() THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2))
				END FutureAppointmentsPercentage,
				SUM(CASE WHEN ACD.AbsureStatus = 3 THEN 1 ELSE 0 END) Cancelled,
				CASE 
					WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
						THEN 0 
					ELSE
						CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus = 3 THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2))
				END CancelledPercentage,
				COUNT(DISTINCT AP.AbsureCarId) Rescheduled ,
				CASE 
					WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
						THEN 0 
					ELSE
						CAST(CAST(COUNT(DISTINCT AP.AbsureCarId) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2))
				END RescheduledPercentage,
				CASE 
					WHEN SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) = 0 
						THEN 0 
					ELSE
						CAST(CAST((SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) AND ACD.SurveyDate > ACD.AppointmentDate THEN 1 ELSE 0 END)) AS float) /
						(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END))*100 AS DECIMAL(8,2))
				END delay  INTO AxaTableData
				FROM TC_Users TU WITH (NOLOCK) 
				INNER JOIN AxaTempTable AS TT ON TT.UserId=TU.Id AND TT.IsAgency <> 1
				LEFT JOIN AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON CSM.TC_UserId = TU.Id 
				LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = CSM.AbSure_CarDetailsId
				LEFT JOIN Absure_Appointments AP WITH (NOLOCK) ON AP.AbsureCarId = CSM.AbSure_CarDetailsId
				GROUP BY  TT.HierarchyRank,TU.Id,--TU.UserName,
				ISNULL(TU.IsAgency,0)



		;WITH CtePerformance  -- get all data related agency ... 
			AS(		     
				 SELECT  ATT.IsAgency,ATT.UserId AS Id, TU.UserName,SUM(Allocated) Allocated ,SUM(Pending) AS Pending ,
				 
				  CASE 
					WHEN COUNT(TD.HierarchyRank)= 0 
						THEN 0 
					ELSE
				    CAST(ROUND(CAST(SUM(PendingPercentage) AS float)/COUNT(TD.HierarchyRank),0) AS INT)  END PendingPercentage,
				 
				    SUM(Done) Done,
					CASE 
					WHEN COUNT(TD.HierarchyRank)= 0 
						THEN 0 
					ELSE
				    CAST(ROUND(CAST(SUM(DonePercentage) AS float)/COUNT(TD.HierarchyRank),0) AS INT)  END DonePercentage,
				 
					SUM(FutureAppointments) FutureAppointments,
					CASE 
					WHEN COUNT(TD.HierarchyRank)= 0 
						THEN 0 
					ELSE
				    CAST(ROUND(CAST(SUM(FutureAppointmentsPercentage) AS float)/COUNT(TD.HierarchyRank),0) AS INT) END FutureAppointmentsPercentage,

					SUM(Cancelled) AS Cancelled,
					CASE 
					WHEN COUNT(TD.HierarchyRank)= 0 
						THEN 0 
					ELSE
				    CAST(ROUND(CAST(SUM(CancelledPercentage) AS float) /COUNT(TD.HierarchyRank),0) AS INT) END CancelledPercentage,


					SUM(Rescheduled) Rescheduled,
					CASE 
					WHEN COUNT(TD.HierarchyRank)= 0 
						THEN 0 
					ELSE
				    CAST(ROUND(CAST(SUM(RescheduledPercentage)  AS float)/COUNT(TD.HierarchyRank),0) AS INT) END RescheduledPercentage,


					CAST(ROUND(SUM(delay),0) AS INT) delay,					

					COUNT(TD.HierarchyRank) TotalSurveyors

				 FROM AxaTableData AS TD
				 LEFT JOIN AxaTempTable AS ATT ON ATT.HierarchyRank=TD.HierarchyRank
				 LEFT JOIN TC_Users AS TU WITH(NOLOCK) ON TU.Id= ATT.UserId
 				 WHERE ATT.IsAgency=1 AND ATT.lvl=2
				 GROUP BY TD.HierarchyRank,ATT.UserId,TU.UserName,ATT.IsAgency  
				 
				)
			 
			--  This is used for pagination 
			  SELECT *, ROW_NUMBER() OVER (ORDER BY Id ) NumberForPaging INTO   #TblTemp FROM   CtePerformance 
	 
			  SELECT * FROM #TblTemp WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	
			  SELECT COUNT(*) AS RecordCount 
			  FROM #TblTemp 
	          
		   --   DROP TABLE #AxaTableData
			  --DROP TABLE #TblTemp 
		  END
		ELSE
			BEGIN 
				 ;WITH CtePerformance  -- get all data related agency ... 
					AS(
						SELECT ISNULL(TU.IsAgency,0) IsAgency ,TU.Id ,TU.UserName,COUNT(DISTINCT CSM.AbSure_CarDetailsId) Allocated,
						SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END) Pending,
						CASE 
							WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2)),0) AS INT)
						END PendingPercentage,
						SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) Done,
						CASE 
							WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2)),0) AS INT)
						END DonePercentage,
						SUM(CASE WHEN ACD.AbsureStatus IS NULL AND acd.AppointmentDate >= GETDATE() THEN 1 ELSE 0 END) FutureAppointments,
						CASE 
							WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND acd.AppointmentDate >= GETDATE() THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2)),0) AS INT)
						END FutureAppointmentsPercentage,
						SUM(CASE WHEN ACD.AbsureStatus = 3 THEN 1 ELSE 0 END) Cancelled,
						CASE 
							WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST(SUM(CASE WHEN ACD.AbsureStatus = 3 THEN 1 ELSE 0 END) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2)),0) AS INT)
						END CancelledPercentage,
						COUNT(DISTINCT AP.AbsureCarId) Rescheduled ,
						CASE 
							WHEN COUNT(DISTINCT CSM.AbSure_CarDetailsId)=0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST(COUNT(DISTINCT AP.AbsureCarId) AS float) / COUNT(DISTINCT CSM.AbSure_CarDetailsId) *100 AS DECIMAL(8,2)),0) AS INT)
						END RescheduledPercentage,
						CASE 
							WHEN SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END) = 0 
								THEN 0 
							ELSE
								CAST(ROUND(CAST(CAST((SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) AND ACD.SurveyDate > ACD.AppointmentDate THEN 1 ELSE 0 END)) AS float) /
								(SUM(CASE WHEN ACD.AbsureStatus IS NULL AND ACD.AppointmentDate < GETDATE() THEN 1 ELSE 0 END)+SUM(CASE WHEN ACD.AbsureStatus IN (1,2) THEN 1 ELSE 0 END))*100 AS DECIMAL(8,2)),0) AS INT)
						END delay
						FROM TC_Users TU WITH (NOLOCK)
						LEFT JOIN AbSure_CarSurveyorMapping CSM WITH (NOLOCK) ON CSM.TC_UserId = TU.Id 
						LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = CSM.AbSure_CarDetailsId
						LEFT JOIN Absure_Appointments AP WITH (NOLOCK) ON AP.AbsureCarId = CSM.AbSure_CarDetailsId
						WHERE  TU.Id IN(SELECT ChildId FROM TempAxaTable)			
						GROUP BY  TU.Id,TU.UserName,ISNULL(TU.IsAgency,0)		
						)
	   
			--  This is used for pagination 
			  SELECT *, ROW_NUMBER() OVER (ORDER BY Id ) NumberForPaging INTO   #TblTemp1 FROM   CtePerformance 
			 
			  SELECT * FROM #TblTemp1 WHERE (@FromIndex IS NULL AND @ToIndex IS NULL) OR (NumberForPaging  BETWEEN @FromIndex AND @ToIndex )
	
			  SELECT COUNT(*) AS RecordCount 
			  FROM #TblTemp1 

			  DROP TABLE #TblTemp1 
	   
	    END
END
