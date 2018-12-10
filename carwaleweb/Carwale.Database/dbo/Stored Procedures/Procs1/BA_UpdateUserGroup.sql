IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_UpdateUserGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_UpdateUserGroup]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_UpdateUserGroup]
@GroupId INT,
@GroupName VARCHAR(50) = NULL,
@DealerIds VARCHAR(200) = NULL,
@NonC_Names VARCHAR(500) = NULL,
@NonC_Contact VARCHAR(1000) = NULL
--@IsActive BIT = NULL

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DeletedOn DATETIME = NULL
	DECLARE @Status BIT = 0 

	--IF @GroupName <> NULL 
	--BEGIN
IF  (SELECT COUNT(ID) FROM [dbo].[BA_Groups] AS BG WITH (NOLOCK) WHERE BG.GroupName = @GroupName AND BG.ID != @GroupId) = 0  --Check for Same GroupName Exists.
	BEGIN
	---Update Group Table
	UPDATE [dbo].[BA_Groups]
			SET [GroupName] = @GroupName ,[ModifyDate] = GETDATE()
			 WHERE ID = @GroupId



---Delete from  BA_GroupDetails table for this groupId
DELETE FROM  [dbo].[BA_GroupDetails] WHERE GroupId = @GroupId


--Insert Into groupDetails table || [IsCarWaleDealer] = 1
 INSERT INTO [dbo].[BA_GroupDetails]

           ([GroupId]
           ,[DealerId]
           ,[ContactName]
           ,[IsCarWaleDealer]
           ,[IsActive])

		   SELECT @GroupId,D.ID,(D.FirstName +' '+D.LastName) AS Name, 1, 1 FROM Dealers AS D WITH (NOLOCK)
													WHERE D.ID IN (SELECT DL.ListMember FROM [dbo].fnSplitCSVValuesWithIdentity(@DealerIds) AS DL)

--Insert Non carwale Dealer || [IsCarWaleDealer] = 0
INSERT INTO [dbo].[BA_GroupDetails]

           ([GroupId]
           ,[DealerId]
           ,[ContactName]
           ,[IsCarWaleDealer]
           ,[IsActive])

		   SELECT @GroupId,BNC.ID,BNC.Name, 0, 1 FROM BA_NonCarWaleDealer AS BNC 
		   INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Contact) as Mob ON Mob.ListMember = BNC.Mobile
		   INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Names) as Cont ON Cont.ListMember = BNC.Name  



SET @Status  = 1 --If Success

END
ELSE
SET @Status  = 2 --If Same group Name Exists.

SELECT @Status AS Status ---Return status
END
