IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUserContacts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUserContacts]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 11-Jun-14
-- Description:	Get the Contacts for the User
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUserContacts]
@BrokerId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT BCD.DealerId AS DealerId  FROM BA_Contacts AS  BC WITH(NOLOCK) 
	INNER JOIN BA_ContactDetails AS BCD  WITH(NOLOCK) ON BC.ID = BCD.ContactId 
	WHERE  BC.BrokerId = @BrokerId AND BCD.IsCarWaleDealer = 1 AND BCD.IsActive = 1 AND BC.IsActive = 1

	SELECT BN.Name AS Name, BN.Mobile AS Mobile FROM  BA_Contacts AS BC WITH(NOLOCK)
	INNER JOIN BA_ContactDetails AS BCD WITH(NOLOCK)  ON BC.ID = BCD.ContactId
	INNER JOIN BA_NonCarWaleDealer AS BN WITH(NOLOCK) ON BN.ID = BCD.DealerId AND BN.IsActive =1 AND BCD.IsActive = 1 AND BCD.IsCarWaleDealer = 0 AND BC.IsActive = 1
	WHERE BC.BrokerId = @BrokerId

	--select * from BA_Contacts
	--select * from BA_ContactDetails
END
