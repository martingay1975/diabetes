import * as Crypto from "expo-crypto";
import { CryptoDigestAlgorithm } from "expo-crypto";
import api from "../diabetes-fetch";

export interface INightscoutEntry {
	_id: string;
	Sgv: number;
	Systime: number;
	Delta: number;
	Direction: number;
}

export class NightscoutClient {
	private baseUrl: string;

	public constructor(url: string) {
		this.baseUrl = `${url}/api/v2`;
	}

	private getSha1 = async (plainText: string): Promise<string> => {
		return await Crypto.digestStringAsync(CryptoDigestAlgorithm.SHA1, plainText);
		//return Crypto.createHash("sha1").update(plainText).digest("hex").toLowerCase();
	};

	public getEntries = async (apiKey: string): Promise<INightscoutEntry[] | { error: string }> => {
		const ret = await api<INightscoutEntry[], undefined>(`${this.baseUrl}/entries?count=500`, "GET", undefined, {
			"API-SECRET": await this.getSha1(apiKey),
		});
		return ret;
	};
}
