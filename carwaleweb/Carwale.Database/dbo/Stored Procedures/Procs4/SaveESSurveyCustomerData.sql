IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveESSurveyCustomerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveESSurveyCustomerData]
	
GO

/****** Object:  StoredProcedure [dbo].[SaveESSurveyCustomerData]    Script Date: 10/10/2016 2:39:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Created By: Rakesh Yadav ON 6 Oct 2016
--Save Es Survey forms data
CREATE PROCEDURE [dbo].[SaveESSurveyCustomerData] @Name VARCHAR(50) = NULL
	,@Email VARCHAR(50) = NULL
	,@Mobile VARCHAR(10) = NULL
	,@SurveyResponse VARCHAR(200)
	,@Platform INT
	,@CampaignId INT
	,@CustomerId INT = - 1
AS

BEGIN
	IF @CustomerId IS NULL
		OR @CustomerId <= 0
	BEGIN
		INSERT INTO CW_ESSurveyCustomer (
			NAME
			,Email
			,Source
			,EntryDate
			,CityId
			,ContactNumber
			)
		VALUES (
			@Name
			,@Email
			,@Platform
			,GETDATE()
			,NULL
			,@Mobile
			)

		SET @CustomerId = SCOPE_IDENTITY()

		IF @CustomerId IS NOT NULL
			AND @CustomerId > 0
		BEGIN
			INSERT INTO CW_ESSurveyCustomerAnswers (
				CustomerId
				,QuestionId
				,OptionId
				,EntryDate
				,ESSurveyCampaignId
				)
			SELECT @CustomerId
				,Val1
				,Val2
				,GETDATE()
				,@CampaignId
			FROM [dbo].[SplitTextByTwoDelimiters](@SurveyResponse, ',', '~');
		END
	END
	ELSE
	BEGIN
		UPDATE CW_ESSurveyCustomer
		SET NAME = @Name
			,Email = @Email
			,ContactNumber = @Mobile
		WHERE Id = @CustomerId
	END

	SELECT @CustomerId AS CustomerId
END

GO