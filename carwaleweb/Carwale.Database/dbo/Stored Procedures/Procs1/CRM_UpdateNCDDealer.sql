IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateNCDDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateNCDDealer]
GO

	-- =============================================
-- Author:		<Khushaboo patil>
-- Create date: <19/11/2013>
-- Description:	<Update NCD Dealers and NCD_Users>
-- Modified By Ashish Verma on <19/2/2014> : Passed IsPremium Column Constraint
-- =============================================
CREATE PROCEDURE [dbo].[CRM_UpdateNCDDealer]
	@DealerId INT,
	@UId VARCHAR(50),
	@Pwd VARCHAR(50),
	@CtDate DATETIME,
	@IsActive SMALLINT,
	@Url VARCHAR(100),
	@Longitude VARCHAR(20),
	@Lattitude VARCHAR(20),
	@IsPanelOnly BIT,
	@IsPremium BIT=0,
	@IsMVL     BIT=0,
	@TCDealerId  INT

AS
BEGIN
	UPDATE NCD_Dealers
             SET UserId = @UId, Password = @Pwd, JoiningDate = @CtDate, IsActive = @IsActive, NCD_Website = @Url, IsPanelOnly = @IsPanelOnly,
             Longitude = @Longitude, Lattitude = @Lattitude ,IsPremium = @IsPremium ,IsMVL=@IsMVL, TCDealerId=@TCDealerId WHERE DealerId = @DealerID

	UPDATE NCD_Users 
             SET Password=@Pwd ,Email= @UId WHERE DealerId =@DealerID
END
