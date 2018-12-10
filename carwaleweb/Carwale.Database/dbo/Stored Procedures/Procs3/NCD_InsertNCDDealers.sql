IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InsertNCDDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InsertNCDDealers]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th June 2014)
-- Description	:	Insert data into NCD_Dealers table
-- =============================================
CREATE PROCEDURE [dbo].[NCD_InsertNCDDealers]
	
	@DealerId	INT,
	@UserId		VARCHAR(50),
	@Password	VARCHAR(50),
	@CtDate		DATETIME,
	@IsActive	BIT,
	@URL		VARCHAR(50),
	@IsPanelOnly	BIT,
	@Longitude	VARCHAR(20),
	@Lattitude	VARCHAR(20),
	@IsPremium	BIT,
	@IsMvl		BIT,
	@TcDealerId		INT,
	@IsAdded	BIT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @IsAdded = 0
    INSERT INTO NCD_Dealers
					(DealerId,UserId,Password,JoiningDate,IsActive,NCD_Website,IsPanelOnly,Longitude,Lattitude,IsPremium,IsMVL,TCDealerId) 
				VALUES
                    (@DealerId,@UserId,@Password,@CtDate,@IsActive,@Url,@IsPanelOnly,@Longitude,@Lattitude,@IsPremium,0,@TCDealerId)
	IF @@ROWCOUNT <> 0
	BEGIN
		SET @IsAdded = 1
	END
END
