IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertFinancerRequiredDocument]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertFinancerRequiredDocument]
GO

	--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR FinancerReqiredDocument TABLE

CREATE PROCEDURE [dbo].[InsertFinancerRequiredDocument]
	@Id			NUMERIC,
	@FinancerId		 INT,	
	@UserTypeId	  	 INT,
	@isUsed		 BIT,
	@DocumentId		 INT,
	@Comment		VARCHAR(250)
 AS

	
BEGIN
	INSERT INTO FinancerRequiredDocument(FinancerId, UserTypeId, isUsed, DocumentTypeId, Comment, isActive)
	VALUES(@FinancerId, @UserTypeId, @isUsed, @DocumentId, @Comment, 1)	
END