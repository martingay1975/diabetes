import React, { useEffect, useState } from "react";
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert } from "react-native";
import { INightscoutEntry, NightscoutClient } from "../../app/nightscout/nightscoutClient";

interface IStatsProps {
	url: string;
	apiKey: string;
}

const Stats: React.FC<IStatsProps> = (props: IStatsProps) => {
	const [last24HourData, setLast24HourData] = useState<INightscoutEntry[]>([]);

	// useEffect(() => {
	// 	const fetchDataPromise = async () => {
	// 		const nightscoutClient = new NightscoutClient(props.url);
	// 		const entries = await nightscoutClient.getEntries(props.apiKey);
	// 		const arr = entries as INightscoutEntry[];
	// 		setLast24HourData(arr);
	// 	};

	// 	fetchDataPromise();
	// }, []);

	return (
		<View>
			<Text>{props.url}</Text>
			<Text>{props.apiKey}</Text>
			<Text>{last24HourData.length}</Text>
		</View>
	);
};

export default Stats;
