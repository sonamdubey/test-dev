IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_SaveUserContacts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_SaveUserContacts]
GO

	-- =============================================
-- Author:		Ranjeet
-- Create date: 06-Jun-14
-- Description:	Save the contacts of Broker.
-- =============================================
CREATE PROCEDURE [dbo].[BA_SaveUserContacts]
@BrokerId INT,
@DealerIds VARCHAR(200) = NULL,
@NonC_Names VARCHAR(500) = NULL,
@NonC_Contact VARCHAR(1000) = NULL

AS
BEGIN
	SET NOCOUNT ON;
	--DECLARE @DeletedOn DATETIME = NULL
	DECLARE @Status BIT = 0 
	DECLARE @ContactId INT = NULL

	---Insert Non CarWale Contacts
	INSERT INTO [dbo].[BA_NonCarWaleDealer]
           ([Mobile] ,[Name],[Address],[CreatedDate],[ModifyDate],[IsActive],[BrokerId])
			(select CAST(Mob.ListMember AS bigint),NM.ListMember,NULL, GETDATE(),NULL, 1,@BrokerId  from [dbo].fnSplitCSVValuesWithIdentity(@NonC_Contact) as Mob
			INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Names) as NM ON NM.id = Mob.id 
			WHERE NOT EXISTS(SELECT BNC.Mobile
                    FROM BA_NonCarWaleDealer AS BNC WITH (NOLOCK)
                   WHERE Mob.ListMember = BNC.Mobile))
	
--Check if BrokerId already exist in table			
 --Insert Into contacts table || [IsCarWaleDealer] = 1
 IF (SELECT COUNT(BC.Id) FROM  BA_Contacts AS BC WITH (NOLOCK) WHERE BC.BrokerId = @BrokerId) = 0
	BEGIN
	INSERT INTO [dbo].[BA_Contacts]
		       ([BrokerId],[CreatedOn],[ModifyDate] ,[DeletedOn],[IsActive])
		VALUES
				(@BrokerId,GETDATE(),NULL,NULL,1)		
		SET @ContactId =  SCOPE_IDENTITY();
	END
ELSE --update
BEGIN
UPDATE  [dbo].[BA_Contacts] SET [ModifyDate] = GETDATE() WHERE BRokerId = @BrokerId
SELECT @ContactId = Id FROM [dbo].[BA_Contacts] WHERE  BRokerId = @BrokerId
END

--Insert Non-CarWale List
INSERT INTO [dbo].[BA_ContactDetails]
           ([ContactId]
           ,[DealerId]
           ,[Email]
           ,[ContactName]
           ,[IsCarWaleDealer]
           ,[IsActive])

		  ( SELECT @ContactId,BNC.ID,NULL,BNC.Name, 0, 1 FROM BA_NonCarWaleDealer AS BNC WITH(NOLOCk) 
		   INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Contact) as Mob ON Mob.ListMember = BNC.Mobile
		   INNER JOIN  [dbo].fnSplitCSVValuesWithIdentity(@NonC_Names) as Cont ON Cont.ListMember = BNC.Name AND BNC.BrokerId = @BrokerId 
		   WHERE NOT EXISTS(SELECT BCD.ID
                    FROM BA_ContactDetails AS BCD WITH (NOLOCK)
                   WHERE BNC.ID = BCD.DealerId AND BCD.IsCarWaleDealer = 0 ))
			
--Insert CarWale List
INSERT INTO [dbo].[BA_ContactDetails]
           ([ContactId]
           ,[DealerId]
           ,[IsCarWaleDealer]
           ,[IsActive])

		  ( SELECT @ContactId,DL.ListMember, 1, 1 FROM  [dbo].fnSplitCSVValuesWithIdentity(@DealerIds) as DL 
		  WHERE NOT EXISTS(SELECT BCD.ID
                    FROM BA_ContactDetails AS BCD WITH (NOLOCK)
                   WHERE DL.ListMember = BCD.DealerId AND BCD.IsCarWaleDealer = 1 ))
		  

SET @Status  = 1 --If Success

SELECT @Status AS Status ---Return status
 
END


/****** Object:  StoredProcedure [dbo].[BA_EditUsedCarStock]    Script Date: 6/9/2014 7:06:59 PM ******/
SET ANSI_NULLS ON
