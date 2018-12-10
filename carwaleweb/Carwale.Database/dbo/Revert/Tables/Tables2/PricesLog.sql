IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'PricesLog'
			AND xtype = 'U'
		)
BEGIN
	DROP TABLE [PricesLog]
END