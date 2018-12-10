IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_AddToUserGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_AddToUserGroup]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 28-May-14
-- Description:	Add contacts to the Group.
-- =============================================
CREATE PROCEDURE [dbo].[BA_AddToUserGroup]
@GroupId INT,
@DealerIds VARCHAR(200) = NULL,
@NonC_Names VARCHAR(500) = NULL,
@NonC_Contact VARCHAR(1000) = NULL

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DeletedOn DATETIME = NULL
	DECLARE @Status BIT = 0 

	
	---Insert Non CarWale Contacts
	INSERT INTO [dbo].[BA_NonCarWaleDealer]
           ([Mobile]
           ,[Name]
           ,[Address]
           ,[CreatedDate]
           ,[ModifyDate]
           ,[IsActive]
		   ,[BrokerId])
		 
			(select CAST(Mob.ListMember AS bigint),NM.ListMember,NULL, GETDATE(),NULL, 1, BG.BrokerId 
			from [dbo].fnSplitCSVValuesWithIdentity(@NonC_Contact) as Mob
			INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Names) as NM ON NM.id = Mob.id
			INNER JOIN BA_Groups AS BG WITH(NOLOCK) ON BG.ID = @GroupId
			WHERE NOT EXISTS(SELECT BNC.Mobile
                    FROM BA_NonCarWaleDealer AS BNC WITH (NOLOCK)
                   WHERE Mob.ListMember = BNC.Mobile))

 --Insert Into groupDetails table || [IsCarWaleDealer] = 1
 INSERT INTO [dbo].[BA_GroupDetails]

           ([GroupId]
           ,[DealerId]
           ,[IsCarWaleDealer]
           ,[IsActive])

		   SELECT @GroupId,D.ID, 1, 1 FROM Dealers AS D WITH(NOLOCK)
									WHERE D.ID IN  (SELECT DL.ListMember FROM [dbo].fnSplitCSVValuesWithIdentity(@DealerIds) AS DL WHERE
									NOT EXISTS(SELECT BGD.ID FROM BA_GroupDetails AS BGD WITH (NOLOCK)  WHERE DL.ListMember = BGD.DealerId AND BGD.GroupId=@GroupId ))

INSERT INTO [dbo].[BA_GroupDetails]
           ([GroupId]
           ,[DealerId]
           ,[IsCarWaleDealer]
           ,[IsActive])
		   SELECT @GroupId,BNC.ID, 0, 1 FROM BA_NonCarWaleDealer AS BNC WITH(NOLOCK) 
		   INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Contact) as Mob  ON Mob.ListMember = BNC.Mobile
		   --INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Names) as Cont ON Cont.ListMember = BNC.Name 
		   INNER JOIN BA_Groups AS BG WITH(NOLOCK) ON BG.ID = @GroupId AND BNC.BrokerId = BG.BrokerId 

SET @Status  = 1 --If Success

SELECT @Status AS Status ---Return status
 
END


/****** Object:  StoredProcedure [dbo].[BA_LoginStatus]    Script Date: 6/10/2014 3:12:59 PM ******/
SET ANSI_NULLS ON
