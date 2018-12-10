IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveReportProblems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveReportProblems]
GO

	

-- =============================================
-- Author:      Vinay Kumar Prajapati  14th   Aug  2015
-- Description:    To save AbSure report problem (from absureApp)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveReportProblems]
	@IsProblem                    BIT ,
	@Comment                      NVARCHAR(800) = null,
	@ImageCount                   Int,
    @UserId                       INT,
	@AppVersion                   VARCHAR(40) = null,
	@PhoneApiLevel                VARCHAR(100) = null,
    @PhoneImei                    VARCHAR(100) = null,
    @PhoneMaufacturer             VARCHAR(100) = null ,
	@PhoneModel                   VARCHAR(100)= null,
    @Absure_ReportProblemsId      INT  OUTPUT
   
AS
DECLARE @CityId INT =null
DECLARE @AreaId INT =null
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON;
	IF @UserId IS NOT NULL
		BEGIN

		     SELECT @CityId=TU.CityId,@AreaId=TU.AreaId FROM TC_Users AS TU WITH(NOLOCK) WHERE Id = @UserId

			INSERT INTO Absure_ReportProblems(SurveyorId,IsProblem,Comment,AppVersion,PhoneApilevel,PhoneImei,PhoneManufacturer,PhoneModel,CityId,AreaId,EntryDate,ImageCount) 
			VALUES(@UserId,@IsProblem,@Comment,@AppVersion,@PhoneApiLevel,@PhoneImei,@PhoneMaufacturer,@PhoneModel,@CityId,@AreaId,GETDATE(),@ImageCount) 
			SET @Absure_ReportProblemsId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
		   SET @Absure_ReportProblemsId =-1
		END
END


