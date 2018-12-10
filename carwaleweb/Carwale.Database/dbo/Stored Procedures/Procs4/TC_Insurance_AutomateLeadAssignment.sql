IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_AutomateLeadAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_AutomateLeadAssignment]
GO

	-- =============================================   
-- Author:  Kritika Choudhary 
-- Create date: 20th Oct 2016   
-- Description:assign leads to TL in RR fashion
-- exec [TC_Insurance_AutomateLeadAssignment] 20553,16
-- =============================================   
CREATE  PROCEDURE [dbo].[TC_Insurance_AutomateLeadAssignment] 
@BranchId INT,
@ReminderId INT = NULL
AS 
BEGIN 
	      DECLARE @NumOfLeads INT,
		          @NumOfTL INT,
				  @UserId INT,
				  @Count INT,
				  @FromIndex INT = 1,
				  @ToIndex INT,
				  @Remainder INT

		  CREATE TABLE #TempLeads (TC_Insurance_ReminderId int,Id int IDENTITY(1,1));
		  
		  INSERT INTO #TempLeads
			SELECT	TC_Insurance_ReminderId
			FROM	TC_Insurance_Reminder WITH(NOLOCK)
			WHERE	DATEDIFF(DD, GETDATE(),ExpiryDate) <= (CASE WHEN @ReminderId IS NULL THEN  60 ELSE 0 END )
					AND (@ReminderId IS NULL OR TC_Insurance_ReminderId = @ReminderId)
					AND BranchId = @BranchId 
					AND ISNULL(AssignedTo,0) = 0

		  SET @NumOfLeads = @@ROWCOUNT
		  
		  IF(@NumOfLeads > 0)
		  BEGIN
		       CREATE TABLE #TempUsers (UserId int,Id int IDENTITY(1,1));

			   INSERT INTO #TempUsers
			   SELECT UR.UserId
			   FROM Dealers D WITH(NOLOCK)
			   JOIN TC_Users U WITH(NOLOCK) ON D.ID=U.BranchId
			   JOIN TC_UsersRole UR WITH(NOLOCK) ON U.Id = UR.UserId
			   WHERE D.id=@BranchId AND U.IsActive=1 AND UR.RoleId=21 --Team Lead

			   SET @NumOfTL = @@ROWCOUNT

			  IF(@NumOfTL > 1)
			  BEGIN 
			  
				SET @Count = (@NumOfLeads / @NumOfTL)
				SET @Remainder = (@NumOfLeads % @NumOfTL)
				SET @ToIndex = @Count + CASE WHEN (@Remainder > 0) THEN 1 ELSE 0 END
			
				WHILE (@NumOfTL > 0)
				BEGIN
				  	SET @UserId = (SELECT UserId FROM #TempUsers WHERE Id = @NumOfTL)
					SELECT 1
					UPDATE TC_Insurance_Reminder
					SET AssignedTo = @UserId
					WHERE TC_Insurance_ReminderId IN (SELECT TC_Insurance_ReminderId FROM #TempLeads WHERE Id BETWEEN @FromIndex AND @ToIndex)
					
					SET @NumOfTL = @NumOfTL - 1
					SET @FromIndex = @ToIndex + 1
					SET @Remainder = @Remainder - 1 
					SET @ToIndex = @ToIndex + @Count + CASE WHEN (@Remainder > 0) THEN 1 ELSE 0 END
					
				END
			  END
			  ELSE
			  BEGIN
				UPDATE TC_Insurance_Reminder
				SET AssignedTo = (SELECT UserId FROM #TempUsers)
				WHERE TC_Insurance_ReminderId IN (SELECT TC_Insurance_ReminderId FROM #TempLeads)
			  END
			  DROP TABLE #TempUsers
		  END
		  DROP TABLE #TempLeads
END


