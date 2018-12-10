IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_updateAgency]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_updateAgency]
GO

	
CREATE     PROCEDURE CMS_updateAgency

@Name			VARCHAR(200),
@Aliases		VARCHAR(500),
@ContactPerson	VARCHAR(500),
@ContactNumber	VARCHAR(200),
@ContactEmail		VARCHAR(100),
@OtherDetails		VARCHAR(2000),
@IsActive		BIT,
@Id			BIGINT

AS	
BEGIN

	UPDATE CMS_Agencies SET [Name]=@Name, Aliases=@Aliases, 
		ContactPerson = @ContactPerson, ContactNumber =@ContactNumber,
		ContactEmail = @ContactEmail, OtherDetails = @OtherDetails,
		IsActive = @IsActive
	WHERE id = @Id

END
