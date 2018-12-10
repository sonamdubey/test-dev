IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'PQ_PriceAvailabilityStatus'
			AND xtype = 'U'
		)
BEGIN
	DROP TABLE PQ_PriceAvailabilityStatus
END